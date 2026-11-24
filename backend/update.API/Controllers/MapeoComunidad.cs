using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;

namespace update.API.Controllers;

internal static class UrlsArchivo
{
    public static string? Publica(this HttpRequest peticion, string? clave)
        => clave is null ? null : $"{peticion.Scheme}://{peticion.Host}/uploads/{clave}";

    public static string? Firmada(this HttpRequest peticion, string? clave, FirmaArchivos firmas)
        => clave is null ? null : $"{peticion.Publica(clave)}?{firmas.Firmar(clave)}";
}

/// <summary>Convierte los resultados del feed y de la comunidad en las respuestas JSON; `url` arma la URL pública de un archivo.</summary>
internal static class MapeoComunidad
{
    public static RespuestaPublicacionFeedDto Publicacion(PublicacionFeed resultado, Guid usuarioId, Func<string?, string?> url, Func<string?, string?> urlFirmada) => new()
    {
        Id = resultado.Publicacion.Id,
        Contenido = resultado.Publicacion.Contenido,
        ImagenArchivoId = resultado.ImagenArchivoId,
        AdjuntoTipoMime = resultado.TipoMime,
        ImagenUrl = resultado.Autor.EsPrivado ? urlFirmada(resultado.ClaveImagen) : url(resultado.ClaveImagen),
        Autor = Autor(resultado.Autor, resultado.ClaveFotoAutor, url),
        EsMia = resultado.Publicacion.CreadoPorUsuarioId == usuarioId,
        TotalReacciones = resultado.Publicacion.TotalReacciones,
        TotalComentarios = resultado.Publicacion.TotalComentarios,
        MeGusta = resultado.MeGusta,
        CreadoEn = resultado.Publicacion.CreadoEn,
        EditadoEn = resultado.Publicacion.EditadoEn
    };

    public static RespuestaComentarioFeedDto Comentario(ComentarioVista vista, Func<string?, string?> url) => new()
    {
        Id = vista.Comentario.Id,
        Contenido = vista.Comentario.Contenido,
        Autor = Autor(vista.Autor, vista.ClaveFotoAutor, url),
        PuedeEliminar = vista.PuedeEliminar,
        CreadoEn = vista.Comentario.CreadoEn
    };

    public static PerfilPublicoDto Perfil(PerfilVista vista, Func<string?, string?> url) => new()
    {
        NombreUsuario = vista.Perfil.NombreUsuario,
        NombreVisible = vista.Perfil.NombreVisible,
        Titular = vista.Perfil.Titular,
        Biografia = vista.Perfil.Biografia,
        SitioWeb = vista.Perfil.SitioWeb,
        FotoUrl = url(vista.ClaveFoto),
        TotalPublicaciones = vista.Perfil.TotalPublicaciones,
        TotalSeguidores = vista.Perfil.TotalSeguidores,
        TotalSeguidos = vista.Perfil.TotalSeguidos,
        EsMio = vista.EsMio,
        LoSigo = vista.LoSigo,
        SolicitudPendiente = vista.SolicitudPendiente,
        EsPrivado = vista.Perfil.EsPrivado,
        PuedeVerContenido = vista.PuedeVerContenido
    };

    private static AutorPublicacionDto Autor(Perfil perfil, string? claveFoto, Func<string?, string?> url) => new()
    {
        NombreUsuario = perfil.NombreUsuario,
        NombreVisible = perfil.NombreVisible,
        FotoUrl = url(claveFoto)
    };
}
