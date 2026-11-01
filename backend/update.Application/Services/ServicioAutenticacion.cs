using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Application.DTOs;
using update.Domain.Exceptions;

namespace update.Application.Services;

public sealed class AuthService(
    IUsuarioRepository usuarios,
    IRefreshTokenRepository refreshTokens,
    IPasswordHasher<Usuario> passwordHasher,
    TokenService tokenService,
    IUnitOfWork unitOfWork)
{

    public async Task<AuthSessionResult> RegistrarAsync(
    RegistroRequest request,
    CancellationToken cancellationToken)
    {
        var correo = request.Correo.Trim();

        if (await usuarios.CorreoExisteAsync(correo, cancellationToken))
            throw new InvalidOperationException("No se pudo crear la cuenta.");

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Correo = correo,
            RolPlataforma = RolPlataforma.Usuario,
            TerminosAceptadosEn = request.TerminosAceptadosEn
        };

        usuario.HashContrasena = passwordHasher.HashPassword(
            usuario, request.Contrasena);

        usuario.Validar();

        return await unitOfWork.EjecutarEnTransaccionAsync(async ct =>
        {
            await usuarios.AgregarAsync(usuario, ct);
            return await CrearSesionAsync(usuario, ct);
        }, cancellationToken);
    }

    public async Task<AuthSessionResult?> IniciarSesionAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var ahora = DateTime.UtcNow;
        var usuario = await usuarios.ObtenerPorCorreoAsync(request.Correo, cancellationToken);

        if (usuario is null || string.IsNullOrWhiteSpace(usuario.HashContrasena))
            return null; // 401

        // Se comprueba antes de verificar el hash: no se gasta CPU ni se da pistas
        if (usuario.EstaBloqueada(ahora))
            throw new CuentaBloqueadaException(usuario.BloqueadoHasta!.Value);

        var resultado = passwordHasher.VerifyHashedPassword(
            usuario, usuario.HashContrasena, request.Contrasena);

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
            usuario.HashContrasena = passwordHasher.HashPassword(usuario, request.Contrasena);

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

    public async Task<RespuestaUsuarioDto?> ObtenerUsuarioAsync(
        string userId,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(userId, out var id))
            return null;

        var resultados = await usuarios.ObtenerPorIdAsync(
            id, cancellationToken: cancellationToken);

        var usuario = resultados.SingleOrDefault();
        return usuario is null ? null : CrearRespuestaUsuario(usuario);
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

        return new AuthSessionResult(accessToken, CrearRespuestaUsuario(usuario), token, expiraEn);
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

    private static RespuestaUsuarioDto CrearRespuestaUsuario(Usuario usuario)
    {
        return new RespuestaUsuarioDto
        {
            Id = usuario.Id,
            Correo = usuario.Correo,
            RolPlataforma = usuario.RolPlataforma,
            CorreoVerificadoEn = usuario.CorreoVerificadoEn,
            TerminosAceptadosEn = usuario.TerminosAceptadosEn,
            UltimoIngresoEn = usuario.UltimoIngresoEn,
            ContrasenaCambiadaEn = usuario.ContrasenaCambiadaEn,
            CreadoEn = usuario.CreadoEn,
            ActualizadoEn = usuario.ActualizadoEn
        };
    }

}
