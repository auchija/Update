using update.Application.DTOs;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;

namespace update.Application.Services;

public sealed record MiPerfilResultado(Perfil Perfil, string? ClaveFoto);

public sealed class NombreUsuarioNoDisponibleException() : Exception("Ese usuario no está disponible.");

public sealed class ServicioMiPerfil(
    IPerfilRepository perfiles,
    IArchivoRepository archivos,
    IUsuarioRepository usuarios,
    IUsuarioReservadoRepository reservados)
{
    public async Task<MiPerfilResultado?> ObtenerAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        var perfil = await ObtenerOCrearAsync(usuarioId, cancellationToken);
        if (perfil is null) return null;
        return new MiPerfilResultado(perfil, await ObtenerClaveFotoAsync(perfil.FotoArchivoId, cancellationToken));
    }

    public async Task<MiPerfilResultado?> ActualizarAsync(
        Guid usuarioId,
        ActualizarMiPerfilDto entrada,
        CancellationToken cancellationToken)
    {
        var perfil = await ObtenerOCrearAsync(usuarioId, cancellationToken);
        if (perfil is null) return null;

        await ComprobarNombreUsuarioAsync(entrada.NombreUsuario, perfil.Id, cancellationToken);
        var claveFoto = await ObtenerClaveFotoPropiaAsync(entrada.FotoArchivoId, usuarioId, cancellationToken);

        perfil.NombreUsuario = entrada.NombreUsuario;
        perfil.NombreVisible = entrada.NombreVisible.Trim();
        perfil.Titular = TextoOpcional(entrada.Titular);
        perfil.Biografia = TextoOpcional(entrada.Biografia);
        perfil.SitioWeb = TextoOpcional(entrada.SitioWeb);
        perfil.FotoArchivoId = entrada.FotoArchivoId;
        perfil.ActualizadoEn = DateTime.UtcNow;

        await perfiles.ActualizarAsync(perfil, cancellationToken);
        return new MiPerfilResultado(perfil, claveFoto);
    }

    private async Task<Perfil?> ObtenerOCrearAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        var encontrados = await perfiles.BuscarAsync(
            p => p.UsuarioId == usuarioId && p.Tipo == TipoPerfil.Persona, 1, 1, cancellationToken);
        if (encontrados.Count > 0) return encontrados[0];

        var usuario = (await usuarios.ObtenerPorIdAsync(usuarioId, cancellationToken: cancellationToken)).SingleOrDefault();
        if (usuario is null) return null;

        var perfil = new Perfil
        {
            UsuarioId = usuarioId,
            Tipo = TipoPerfil.Persona,
            NombreUsuario = $"u_{usuarioId:N}"[..22],
            NombreVisible = usuario.Correo.Split('@')[0]
        };
        await perfiles.AgregarAsync(perfil, cancellationToken);
        return perfil;
    }

    private async Task ComprobarNombreUsuarioAsync(string nombre, Guid perfilId, CancellationToken cancellationToken)
    {
        if (await reservados.ObtenerPorClaveAsync([nombre], cancellationToken) is not null)
            throw new NombreUsuarioNoDisponibleException();

        var iguales = await perfiles.BuscarAsync(
            p => p.NombreUsuario == nombre && p.Id != perfilId, 1, 1, cancellationToken);
        if (iguales.Count > 0) throw new NombreUsuarioNoDisponibleException();
    }

    private async Task<string?> ObtenerClaveFotoPropiaAsync(Guid? archivoId, Guid usuarioId, CancellationToken cancellationToken)
    {
        if (archivoId is null) return null;

        var archivo = await archivos.ObtenerPorClaveAsync([archivoId.Value], cancellationToken);
        var esValida = archivo is { Proposito: PropositoArchivo.FotoPerfil, Estado: EstadoArchivo.Listo }
            && archivo.SubidoPorUsuarioId == usuarioId;
        if (!esValida) throw new ExcepcionDominio("La foto no existe o no te pertenece.");

        return archivo!.ClaveObjeto;
    }

    private async Task<string?> ObtenerClaveFotoAsync(Guid? archivoId, CancellationToken cancellationToken)
    {
        if (archivoId is null) return null;
        var archivo = await archivos.ObtenerPorClaveAsync([archivoId.Value], cancellationToken);
        return archivo?.ClaveObjeto;
    }

    private static string? TextoOpcional(string? texto) =>
        string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
}
