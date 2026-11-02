using update.Application.DTOs;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;

namespace update.Application.Services;

/// <summary>Publicación lista para responder: autor, clave de su foto y la imagen adjunta, si hay.</summary>
public sealed record PublicacionFeed(Publicacion Publicacion, Perfil Autor, string? ClaveFotoAutor, Guid? ImagenArchivoId, string? ClaveImagen);

public sealed class PublicacionAjenaException() : Exception("Solo puedes cambiar tus propias publicaciones.");

/// <summary>Casos de uso del feed: listar, publicar, editar y eliminar las publicaciones propias.</summary>
public sealed class ServicioFeed(
    IPublicacionRepository publicaciones,
    IPublicacionArchivoRepository publicacionArchivos,
    IArchivoRepository archivos,
    ITipoPublicacionRepository tiposPublicacion,
    IPerfilRepository perfiles,
    ServicioMiPerfil miPerfil,
    IUnitOfWork unitOfWork)
{
    private const string TipoGeneral = "general";

    public async Task<IReadOnlyList<PublicacionFeed>> ListarAsync(int pagina, int tamanoPagina, CancellationToken cancellationToken)
    {
        var recientes = await publicaciones.ObtenerRecientesAsync(pagina, tamanoPagina, cancellationToken);
        var imagenes = (await publicacionArchivos.ObtenerPorPublicacionesAsync(recientes.Select(p => p.Id).ToList(), cancellationToken))
            .GroupBy(pa => pa.PublicacionId)
            .ToDictionary(g => g.Key, g => g.First().Archivo);

        return recientes.Select(p =>
        {
            var imagen = imagenes.GetValueOrDefault(p.Id);
            return new PublicacionFeed(p, p.PerfilAutor, p.PerfilAutor.FotoArchivo?.ClaveObjeto, imagen?.Id, imagen?.ClaveObjeto);
        }).ToList();
    }

    public async Task<PublicacionFeed?> ObtenerAsync(Guid publicacionId, CancellationToken cancellationToken)
    {
        var publicacion = await publicaciones.ObtenerPorClaveAsync([publicacionId], cancellationToken);
        if (publicacion is not { Visibilidad: VisibilidadPublicacion.Publico }) return null;
        var autor = await perfiles.ObtenerPorClaveAsync([publicacion.PerfilAutorId], cancellationToken);
        if (autor is null) return null;

        var fotoAutor = autor.FotoArchivoId is null ? null : await archivos.ObtenerPorClaveAsync([autor.FotoArchivoId.Value], cancellationToken);
        var imagen = (await publicacionArchivos.ObtenerPorPublicacionesAsync([publicacionId], cancellationToken)).FirstOrDefault()?.Archivo;
        return new PublicacionFeed(publicacion, autor, fotoAutor?.ClaveObjeto, imagen?.Id, imagen?.ClaveObjeto);
    }

    public async Task<PublicacionFeed?> CrearAsync(Guid usuarioId, GuardarPublicacionDto entrada, CancellationToken cancellationToken)
    {
        var autor = await miPerfil.ObtenerAsync(usuarioId, cancellationToken);
        if (autor is null) return null;
        var imagen = await ObtenerImagenPropiaAsync(entrada.ImagenArchivoId, usuarioId, null, cancellationToken);

        var publicacion = new Publicacion
        {
            PerfilAutorId = autor.Perfil.Id,
            CreadoPorUsuarioId = usuarioId,
            TipoPublicacionCodigo = TipoGeneral,
            Contenido = entrada.Contenido.Trim(),
            Visibilidad = VisibilidadPublicacion.Publico
        };

        await unitOfWork.EjecutarEnTransaccionAsync(async ct =>
        {
            // El catálogo puede estar vacío en una base nueva; se crea el tipo la primera vez, como los tipos de persona.
            if (await tiposPublicacion.ObtenerPorClaveAsync([TipoGeneral], ct) is null)
                await tiposPublicacion.AgregarAsync(new TipoPublicacion { Codigo = TipoGeneral, Nombre = "General" }, ct);

            await publicaciones.AgregarAsync(publicacion, ct);
            if (imagen is not null)
                await publicacionArchivos.AgregarAsync(new PublicacionArchivo { PublicacionId = publicacion.Id, ArchivoId = imagen.Id }, ct);

            autor.Perfil.TotalPublicaciones++;
            await perfiles.ActualizarAsync(autor.Perfil, ct);
            return true;
        }, cancellationToken);

        return new PublicacionFeed(publicacion, autor.Perfil, autor.ClaveFoto, imagen?.Id, imagen?.ClaveObjeto);
    }

    public async Task<PublicacionFeed?> ActualizarAsync(Guid usuarioId, Guid publicacionId, GuardarPublicacionDto entrada, CancellationToken cancellationToken)
    {
        var publicacion = await ObtenerPropiaAsync(usuarioId, publicacionId, cancellationToken);
        if (publicacion is null) return null;
        var imagen = await ObtenerImagenPropiaAsync(entrada.ImagenArchivoId, usuarioId, publicacionId, cancellationToken);
        var autor = (await miPerfil.ObtenerAsync(usuarioId, cancellationToken))!;

        publicacion.Contenido = entrada.Contenido.Trim();
        publicacion.EditadoEn = DateTime.UtcNow;

        await unitOfWork.EjecutarEnTransaccionAsync(async ct =>
        {
            await publicaciones.ActualizarAsync(publicacion, ct);

            // Se reemplaza la imagen solo si cambió; quitarla borra el vínculo, no el archivo.
            var actuales = await publicacionArchivos.ObtenerPorPublicacionesAsync([publicacionId], ct);
            if (actuales.Select(pa => (Guid?)pa.ArchivoId).FirstOrDefault() != imagen?.Id)
            {
                foreach (var actual in actuales) await publicacionArchivos.EliminarAsync(actual, ct);
                if (imagen is not null)
                    await publicacionArchivos.AgregarAsync(new PublicacionArchivo { PublicacionId = publicacionId, ArchivoId = imagen.Id }, ct);
            }
            return true;
        }, cancellationToken);

        return new PublicacionFeed(publicacion, autor.Perfil, autor.ClaveFoto, imagen?.Id, imagen?.ClaveObjeto);
    }

    public async Task<bool> EliminarAsync(Guid usuarioId, Guid publicacionId, CancellationToken cancellationToken)
    {
        var publicacion = await ObtenerPropiaAsync(usuarioId, publicacionId, cancellationToken);
        if (publicacion is null) return false;
        var autor = (await miPerfil.ObtenerAsync(usuarioId, cancellationToken))!;

        await unitOfWork.EjecutarEnTransaccionAsync(async ct =>
        {
            // Borrado lógico: el contexto convierte el DELETE en eliminado_en = ahora.
            await publicaciones.EliminarAsync(publicacion, ct);
            autor.Perfil.TotalPublicaciones = Math.Max(0, autor.Perfil.TotalPublicaciones - 1);
            await perfiles.ActualizarAsync(autor.Perfil, ct);
            return true;
        }, cancellationToken);
        return true;
    }

    /// <summary>null si no existe; PublicacionAjenaException si existe pero es de otra persona.</summary>
    private async Task<Publicacion?> ObtenerPropiaAsync(Guid usuarioId, Guid publicacionId, CancellationToken cancellationToken)
    {
        var publicacion = await publicaciones.ObtenerPorClaveAsync([publicacionId], cancellationToken);
        if (publicacion is null) return null;
        if (publicacion.CreadoPorUsuarioId != usuarioId) throw new PublicacionAjenaException();
        return publicacion;
    }

    private async Task<Archivo?> ObtenerImagenPropiaAsync(Guid? archivoId, Guid usuarioId, Guid? publicacionId, CancellationToken cancellationToken)
    {
        if (archivoId is null) return null;

        var archivo = await archivos.ObtenerPorClaveAsync([archivoId.Value], cancellationToken);
        var esValida = archivo is { Proposito: PropositoArchivo.Publicacion, Estado: EstadoArchivo.Listo }
            && archivo.SubidoPorUsuarioId == usuarioId;
        if (!esValida) throw new ExcepcionDominio("La imagen no existe o no te pertenece.");

        var usos = await publicacionArchivos.ObtenerPorArchivoIdAsync(archivoId.Value, 1, 1, cancellationToken);
        if (usos.Any(uso => uso.PublicacionId != publicacionId))
            throw new ExcepcionDominio("Esa imagen ya está en otra publicación.");

        return archivo;
    }
}
