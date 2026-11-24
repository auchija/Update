using update.Application.DTOs;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;

namespace update.Application.Services;

public sealed record PublicacionFeed(Publicacion Publicacion, Perfil Autor, string? ClaveFotoAutor, Guid? ImagenArchivoId, string? ClaveImagen, string? TipoMime, bool MeGusta = false);

public sealed class PublicacionAjenaException() : Exception("Solo puedes cambiar tus propias publicaciones.");

public sealed class ServicioFeed(
    IPublicacionRepository publicaciones,
    IPublicacionArchivoRepository publicacionArchivos,
    IArchivoRepository archivos,
    ITipoPublicacionRepository tiposPublicacion,
    IPerfilRepository perfiles,
    IReaccionPublicacionRepository reacciones,
    ISeguimientoRepository seguimientos,
    ServicioMiPerfil miPerfil,
    IUnitOfWork unitOfWork)
{
    private const string TipoGeneral = "general";

    public async Task<IReadOnlyList<PublicacionFeed>> ListarAsync(
        Guid visorUsuarioId, int pagina, int tamanoPagina, CancellationToken cancellationToken,
        bool soloMias = false, Guid? perfilAutorId = null)
    {
        var recientes = await publicaciones.ObtenerRecientesAsync(
            pagina, tamanoPagina, cancellationToken, soloMias ? visorUsuarioId : null, perfilAutorId, visorUsuarioId);
        var ids = recientes.Select(p => p.Id).ToList();
        var imagenes = (await publicacionArchivos.ObtenerPorPublicacionesAsync(ids, cancellationToken))
            .GroupBy(pa => pa.PublicacionId)
            .ToDictionary(g => g.Key, g => g.First().Archivo);
        var conMeGusta = await PublicacionesConMeGustaAsync(visorUsuarioId, ids, cancellationToken);

        return recientes.Select(p =>
        {
            var imagen = imagenes.GetValueOrDefault(p.Id);
            return new PublicacionFeed(p, p.PerfilAutor, p.PerfilAutor.FotoArchivo?.ClaveObjeto, imagen?.Id, imagen?.ClaveObjeto, imagen?.TipoMime, conMeGusta.Contains(p.Id));
        }).ToList();
    }

    public async Task<PublicacionFeed?> ObtenerAsync(Guid publicacionId, Guid usuarioId, CancellationToken cancellationToken)
    {
        var publicacion = await publicaciones.ObtenerPorClaveAsync([publicacionId], cancellationToken);
        if (publicacion is null || (publicacion.Visibilidad != VisibilidadPublicacion.Publico && publicacion.CreadoPorUsuarioId != usuarioId)) return null;
        var autor = await perfiles.ObtenerPorClaveAsync([publicacion.PerfilAutorId], cancellationToken);
        if (autor is null) return null;
        if (autor.EsPrivado && autor.UsuarioId != usuarioId)
        {
            var estados = await seguimientos.ObtenerEstadosAsync(usuarioId, [autor.Id], cancellationToken);
            if (estados.GetValueOrDefault(autor.Id, EstadoSeguimiento.Pendiente) != EstadoSeguimiento.Activo) return null;
        }

        var fotoAutor = autor.FotoArchivoId is null ? null : await archivos.ObtenerPorClaveAsync([autor.FotoArchivoId.Value], cancellationToken);
        var imagen = (await publicacionArchivos.ObtenerPorPublicacionesAsync([publicacionId], cancellationToken)).FirstOrDefault()?.Archivo;
        var meGusta = (await PublicacionesConMeGustaAsync(usuarioId, [publicacionId], cancellationToken)).Count > 0;
        return new PublicacionFeed(publicacion, autor, fotoAutor?.ClaveObjeto, imagen?.Id, imagen?.ClaveObjeto, imagen?.TipoMime, meGusta);
    }

    private async Task<HashSet<Guid>> PublicacionesConMeGustaAsync(Guid usuarioId, IReadOnlyCollection<Guid> publicacionIds, CancellationToken cancellationToken)
    {
        if (publicacionIds.Count == 0) return [];
        var perfilVisor = (await perfiles.BuscarAsync(p => p.UsuarioId == usuarioId && p.Tipo == TipoPerfil.Persona, 1, 1, cancellationToken)).FirstOrDefault();
        if (perfilVisor is null) return [];
        return [.. await reacciones.ObtenerPublicacionesConReaccionAsync(perfilVisor.Id, publicacionIds, cancellationToken)];
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
            if (await tiposPublicacion.ObtenerPorClaveAsync([TipoGeneral], ct) is null)
                await tiposPublicacion.AgregarAsync(new TipoPublicacion { Codigo = TipoGeneral, Nombre = "General" }, ct);

            await publicaciones.AgregarAsync(publicacion, ct);
            if (imagen is not null)
                await publicacionArchivos.AgregarAsync(new PublicacionArchivo { PublicacionId = publicacion.Id, ArchivoId = imagen.Id }, ct);

            autor.Perfil.TotalPublicaciones++;
            await perfiles.ActualizarAsync(autor.Perfil, ct);
            return true;
        }, cancellationToken);

        return new PublicacionFeed(publicacion, autor.Perfil, autor.ClaveFoto, imagen?.Id, imagen?.ClaveObjeto, imagen?.TipoMime);
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

            var actuales = await publicacionArchivos.ObtenerPorPublicacionesAsync([publicacionId], ct);
            if (actuales.Select(pa => (Guid?)pa.ArchivoId).FirstOrDefault() != imagen?.Id)
            {
                foreach (var actual in actuales) await publicacionArchivos.EliminarAsync(actual, ct);
                if (imagen is not null)
                    await publicacionArchivos.AgregarAsync(new PublicacionArchivo { PublicacionId = publicacionId, ArchivoId = imagen.Id }, ct);
            }
            return true;
        }, cancellationToken);

        var meGusta = (await PublicacionesConMeGustaAsync(usuarioId, [publicacionId], cancellationToken)).Count > 0;
        return new PublicacionFeed(publicacion, autor.Perfil, autor.ClaveFoto, imagen?.Id, imagen?.ClaveObjeto, imagen?.TipoMime, meGusta);
    }

    public async Task<bool> EliminarAsync(Guid usuarioId, Guid publicacionId, CancellationToken cancellationToken)
    {
        var publicacion = await ObtenerPropiaAsync(usuarioId, publicacionId, cancellationToken);
        if (publicacion is null) return false;
        var autor = (await miPerfil.ObtenerAsync(usuarioId, cancellationToken))!;

        await unitOfWork.EjecutarEnTransaccionAsync(async ct =>
        {
            await publicaciones.EliminarAsync(publicacion, ct);
            autor.Perfil.TotalPublicaciones = Math.Max(0, autor.Perfil.TotalPublicaciones - 1);
            await perfiles.ActualizarAsync(autor.Perfil, ct);
            return true;
        }, cancellationToken);
        return true;
    }

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
