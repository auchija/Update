using update.Application.DTOs;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;

namespace update.Application.Services;

public sealed class PublicacionNoEncontradaException() : Exception { }
public sealed class PublicacionAjenaException() : Exception { }

public sealed class ServicioPublicaciones(
    IPublicacionRepository publicaciones,
    ITipoPublicacionRepository tipos,
    ServicioMiPerfil miPerfil)
{
    public async Task<IReadOnlyList<PublicacionFeedDto>> ListarAsync(
        Guid usuarioId, int pagina, int tamanoPagina, CancellationToken cancellationToken)
    {
        var lista = await publicaciones.ObtenerFeedAsync(usuarioId, pagina, tamanoPagina, cancellationToken);
        return lista.Select(p => ARespuesta(p, usuarioId)).ToList();
    }

    public async Task<PublicacionFeedDto> CrearAsync(
        Guid usuarioId, GuardarPublicacionFeedDto entrada, CancellationToken cancellationToken)
    {
        var contenido = ValidarContenido(entrada.Contenido);
        var tipo = await tipos.ObtenerPorClaveAsync(["general"], cancellationToken);
        if (tipo is not { Activo: true })
            throw new ExcepcionDominio("Falta habilitar el tipo de publicacion general en la base de datos.");

        var resultado = await miPerfil.ObtenerAsync(usuarioId, cancellationToken);
        if (resultado is null || resultado.Perfil.Estado != EstadoCuenta.Activo)
            throw new ExcepcionDominio("Necesitas un perfil activo para publicar.");

        var publicacion = new Publicacion
        {
            PerfilAutorId = resultado.Perfil.Id,
            CreadoPorUsuarioId = usuarioId,
            Contenido = contenido,
            TipoPublicacionCodigo = "general",
            Visibilidad = VisibilidadPublicacion.Publico
        };
        await publicaciones.AgregarAsync(publicacion, cancellationToken);
        return new PublicacionFeedDto(publicacion.Id, resultado.Perfil.NombreVisible,
            publicacion.Contenido, publicacion.CreadoEn, null, true);
    }

    public async Task<PublicacionFeedDto> ActualizarAsync(
        Guid id, Guid usuarioId, GuardarPublicacionFeedDto entrada, CancellationToken cancellationToken)
    {
        var publicacion = await ObtenerPropiaAsync(id, usuarioId, cancellationToken);
        publicacion.Contenido = ValidarContenido(entrada.Contenido);
        publicacion.EditadoEn = DateTime.UtcNow;
        publicacion.ActualizadoEn = publicacion.EditadoEn.Value;
        await publicaciones.ActualizarAsync(publicacion, cancellationToken);
        return ARespuesta(publicacion, usuarioId);
    }

    public async Task EliminarAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken)
    {
        var publicacion = await ObtenerPropiaAsync(id, usuarioId, cancellationToken);
        publicacion.Eliminar();
        await publicaciones.ActualizarAsync(publicacion, cancellationToken);
    }

    private async Task<Publicacion> ObtenerPropiaAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken)
    {
        var publicacion = await publicaciones.ObtenerConAutorAsync(id, cancellationToken)
            ?? throw new PublicacionNoEncontradaException();
        if (publicacion.CreadoPorUsuarioId != usuarioId) throw new PublicacionAjenaException();
        return publicacion;
    }

    private static string ValidarContenido(string? contenido)
    {
        var texto = contenido?.Trim() ?? "";
        if (texto.Length == 0 || texto.Length > 5000)
            throw new ExcepcionDominio("Escribe entre 1 y 5000 caracteres.");
        return texto;
    }

    private static PublicacionFeedDto ARespuesta(Publicacion p, Guid usuarioId) =>
        new(p.Id, p.PerfilAutor.NombreVisible, p.Contenido, p.CreadoEn,
            p.EditadoEn, p.CreadoPorUsuarioId == usuarioId);
}
