using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Application.DTOs;
using update.Domain.Exceptions;

namespace update.Application.Services;

public sealed class DatosEnUsoException(IReadOnlyDictionary<string, string> errores)
    : Exception("Algunos datos ya están en uso.")
{
    public IReadOnlyDictionary<string, string> Errores { get; } = errores;
}

public sealed class AuthService(
    IUsuarioRepository usuarios,
    IRefreshTokenRepository refreshTokens,
    IPasswordHasher<Usuario> passwordHasher,
    TokenService tokenService,
    IUnitOfWork unitOfWork,
    ServicioMiPerfil miPerfil,
    IPerfilRepository perfiles,
    ITipoPersonaRepository tiposPersona,
    IUsuarioTipoPersonaRepository usuarioTiposPersona)
{
    private static readonly Dictionary<string, string> NombresPersona = new()
    {
        ["entrepreneur"] = "Emprendedor",
        ["buyer"] = "Comprador",
        ["mentor"] = "Mentor",
        ["investor"] = "Inversionista",
        ["supplier"] = "Proveedor",
        ["collaborator"] = "Colaborador"
    };

    public async Task<AuthSessionResult> RegistrarAsync(
    RegistroRequest request,
    CancellationToken cancellationToken)
    {
        var correo = request.Email.Trim().ToLowerInvariant();
        var personas = request.Personas.Distinct().ToList();
        if (personas.Any(codigo => !NombresPersona.ContainsKey(codigo)))
            throw new ExcepcionDominio("Elige una opción válida de cómo participas.");

        var errores = new Dictionary<string, string>();
        if (await usuarios.CorreoExisteAsync(correo, cancellationToken))
            errores["email"] = "Ese correo ya está registrado. ¿Quieres iniciar sesión?";
        if (!await miPerfil.NombreUsuarioDisponibleAsync(request.Handle, Guid.Empty, cancellationToken))
            errores["handle"] = "Ese usuario no está disponible. Prueba con otro.";
        if (errores.Count > 0) throw new DatosEnUsoException(errores);

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Correo = correo,
            RolPlataforma = RolPlataforma.Usuario,
            TerminosAceptadosEn = DateTime.UtcNow
        };

        usuario.HashContrasena = passwordHasher.HashPassword(
            usuario, request.Password);

        usuario.Validar();

        return await unitOfWork.EjecutarEnTransaccionAsync(async ct =>
        {
            await usuarios.AgregarAsync(usuario, ct);
            await perfiles.AgregarAsync(new Perfil
            {
                UsuarioId = usuario.Id,
                Tipo = TipoPerfil.Persona,
                NombreUsuario = request.Handle,
                NombreVisible = request.DisplayName.Trim()
            }, ct);

            foreach (var codigo in personas)
            {
                if (await tiposPersona.ObtenerPorClaveAsync([codigo], ct) is null)
                    await tiposPersona.AgregarAsync(new TipoPersona { Codigo = codigo, Nombre = NombresPersona[codigo] }, ct);
                await usuarioTiposPersona.AgregarAsync(
                    new UsuarioTipoPersona { UsuarioId = usuario.Id, TipoPersonaCodigo = codigo }, ct);
            }

            return await CrearSesionAsync(usuario, ct);
        }, cancellationToken);
    }

    public async Task<AuthSessionResult?> IniciarSesionAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var ahora = DateTime.UtcNow;
        var usuario = await BuscarPorIdentificadorAsync(request.Identifier, cancellationToken);

        if (usuario is null || string.IsNullOrWhiteSpace(usuario.HashContrasena))
            return null; // 401

        // Se comprueba antes de verificar el hash: no se gasta CPU ni se da pistas
        if (usuario.EstaBloqueada(ahora))
            throw new CuentaBloqueadaException(usuario.BloqueadoHasta!.Value);

        var resultado = passwordHasher.VerifyHashedPassword(
            usuario, usuario.HashContrasena, request.Password);

        if (resultado == PasswordVerificationResult.Failed)
        {
            usuario.RegistrarIntentoFallido(ahora);
            await usuarios.ActualizarAsync(usuario, cancellationToken);

            // El intento que dispara el bloqueo ya responde 423
            if (usuario.EstaBloqueada(ahora))
                throw new CuentaBloqueadaException(usuario.BloqueadoHasta!.Value);

            return null; // 401
        }

        // Hash de contraseñas: si Identity cambió sus parámetros, se re-hashea
        if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
            usuario.HashContrasena = passwordHasher.HashPassword(usuario, request.Password);

        usuario.ReiniciarIntentosFallidos();
        usuario.UltimoIngresoEn = ahora;
        await usuarios.ActualizarAsync(usuario, cancellationToken);

        return await CrearSesionAsync(usuario, cancellationToken);
    }
    public async Task<AuthSessionResult?> RefreshAsync(
    string refreshToken,
    CancellationToken cancellationToken)
    {
        var hash = HashearRefreshToken(refreshToken);
        var almacenado = await refreshTokens.ObtenerPorHashAsync(hash, cancellationToken);
        var ahora = DateTime.UtcNow;

        if (almacenado is null || !almacenado.EstaActivo(ahora))
            return null;

        // Revoca el token anterior antes de emitir uno nuevo.
        almacenado.RevocadoEn = ahora;
        await refreshTokens.ActualizarAsync(almacenado, cancellationToken);

        var usuario = await usuarios.ObtenerPorIdAsync(
            almacenado.UsuarioId, cancellationToken: cancellationToken);

        var entidadUsuario = usuario.SingleOrDefault();
        if (entidadUsuario is null || entidadUsuario.EstaBloqueada())
            return null;

        return await CrearSesionAsync(entidadUsuario, cancellationToken);
    }

    public async Task LogoutAsync(
        string refreshToken,
        CancellationToken cancellationToken)
    {
        var almacenado = await refreshTokens.ObtenerPorHashAsync(
            HashearRefreshToken(refreshToken), cancellationToken);

        if (almacenado is null || almacenado.RevocadoEn is not null)
            return;

        almacenado.RevocadoEn = DateTime.UtcNow;
        await refreshTokens.ActualizarAsync(almacenado, cancellationToken);
    }

    public async Task<UsuarioSesionDto?> ObtenerUsuarioAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(userId, out var id))
            return null;

        var resultados = await usuarios.ObtenerPorIdAsync(
            id, cancellationToken: cancellationToken);

        var usuario = resultados.SingleOrDefault();
        return usuario is null ? null : await CrearUsuarioSesionAsync(usuario, cancellationToken);
    }

    private async Task<Usuario?> BuscarPorIdentificadorAsync(
        string identificador,
        CancellationToken cancellationToken)
    {
        var limpio = identificador.Trim().ToLowerInvariant();
        if (!limpio.StartsWith('@') && limpio.Contains('@'))
            return await usuarios.ObtenerPorCorreoAsync(limpio, cancellationToken);

        var nombre = limpio.TrimStart('@');
        var encontrados = await perfiles.BuscarAsync(
            p => p.NombreUsuario == nombre && p.Tipo == TipoPerfil.Persona, 1, 1, cancellationToken);
        if (encontrados.Count == 0) return null;

        var resultados = await usuarios.ObtenerPorIdAsync(
            encontrados[0].UsuarioId, cancellationToken: cancellationToken);
        return resultados.SingleOrDefault();
    }

    private async Task<UsuarioSesionDto> CrearUsuarioSesionAsync(
        Usuario usuario,
        CancellationToken cancellationToken)
    {
        var resultado = await miPerfil.ObtenerAsync(usuario.Id, cancellationToken)
            ?? throw new InvalidOperationException("El usuario no existe.");
        var personas = await usuarioTiposPersona.ObtenerPorUsuarioIdAsync(
            usuario.Id, cancellationToken: cancellationToken);

        return new UsuarioSesionDto(
            usuario.Id,
            usuario.Correo,
            resultado.Perfil.NombreUsuario,
            resultado.Perfil.NombreVisible,
            resultado.ClaveFoto is null ? null : $"/uploads/{resultado.ClaveFoto}",
            personas.Select(p => p.TipoPersonaCodigo).ToList(),
            usuario.RolPlataforma);
    }


    private async Task<AuthSessionResult> CrearSesionAsync(
    Usuario usuario,
    CancellationToken cancellationToken)
    {
        var accessToken = tokenService.CrearAccessToken(usuario);
        var token = CrearRefreshToken();
        var expiraEn = DateTime.UtcNow.Add(DuracionRefreshToken);

        await refreshTokens.AgregarAsync(
            new RefreshToken
            {
                Id = Guid.NewGuid(),
                UsuarioId = usuario.Id,
                Hash = HashearRefreshToken(token),
                ExpiraEn = expiraEn
            },
            cancellationToken);

        return new AuthSessionResult(accessToken, await CrearUsuarioSesionAsync(usuario, cancellationToken), token, expiraEn);
    }

    private static readonly TimeSpan DuracionRefreshToken =
        TimeSpan.FromDays(14);

    private static string CrearRefreshToken() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    private static string HashearRefreshToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(bytes);
    }
}
