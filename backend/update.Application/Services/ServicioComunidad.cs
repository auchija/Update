using update.Application.DTOs;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;

namespace update.Application.Services;

public sealed record ComentarioVista(Comentario Comentario, Perfil Autor, string? ClaveFotoAutor, bool PuedeEliminar);
public sealed record PerfilVista(Perfil Perfil, string? ClaveFoto, bool EsMio, bool LoSigo);
public sealed record ResultadoMeGusta(bool MeGusta, int TotalReacciones);
public sealed record ResultadoSeguimiento(bool LoSigo, int TotalSeguidores);

public sealed class ComentarioAjenoException() : Exception("Solo puedes borrar tus comentarios o los de tus publicaciones.");

/// <summary>Interacción entre usuarios: "me gusta", comentarios, seguir perfiles y perfiles públicos.</summary>
public sealed class ServicioComunidad(
    ServicioFeed feed,
    ServicioMiPerfil miPerfil,
    IPublicacionRepository publicaciones,
    IReaccionPublicacionRepository reacciones,
    ITipoReaccionRepository tiposReaccion,
    IComentarioRepository comentarios,
    IPerfilRepository perfiles,
    ISeguimientoRepository seguimientos,
    IUnitOfWork unitOfWork)
{
    private const string TipoMeGusta = "me_gusta";

    /// <summary>Da o quita el "me gusta". Repetir la misma acción no cambia nada. null si la publicación no se puede ver.</summary>
    public async Task<ResultadoMeGusta?> MarcarMeGustaAsync(Guid usuarioId, Guid publicacionId, bool meGusta, CancellationToken cancellationToken)
    {
        var visible = await feed.ObtenerAsync(publicacionId, usuarioId, cancellationToken);
        var visor = await miPerfil.ObtenerAsync(usuarioId, cancellationToken);
        if (visible is null || visor is null) return null;

        var total = visible.Publicacion.TotalReacciones;
        if (visible.MeGusta == meGusta) return new ResultadoMeGusta(meGusta, total);

        await unitOfWork.EjecutarEnTransaccionAsync(async ct =>
        {
            if (meGusta)
            {
                // El catálogo puede estar vacío en una base nueva; se crea el tipo la primera vez.
                if (await tiposReaccion.ObtenerPorClaveAsync([TipoMeGusta], ct) is null)
                    await tiposReaccion.AgregarAsync(new TipoReaccion { Codigo = TipoMeGusta, Nombre = "Me gusta", Emoji = "❤️", Orden = 1 }, ct);
                await reacciones.AgregarAsync(new ReaccionPublicacion { PublicacionId = publicacionId, PerfilId = visor.Perfil.Id, TipoReaccionCodigo = TipoMeGusta }, ct);
            }
            else
            {
                var existente = await reacciones.ObtenerPorClaveAsync([publicacionId, visor.Perfil.Id], ct);
                if (existente is not null) await reacciones.EliminarAsync(existente, ct);
            }
            await publicaciones.AjustarContadoresAsync(publicacionId, meGusta ? 1 : -1, 0, ct);
            return true;
        }, cancellationToken);

        return new ResultadoMeGusta(meGusta, Math.Max(0, total + (meGusta ? 1 : -1)));
    }

    /// <summary>null si la publicación no se puede ver.</summary>
    public async Task<IReadOnlyList<ComentarioVista>?> ListarComentariosAsync(Guid usuarioId, Guid publicacionId, int pagina, int tamanoPagina, CancellationToken cancellationToken)
    {
        var visible = await feed.ObtenerAsync(publicacionId, usuarioId, cancellationToken);
        if (visible is null) return null;

        var esAutorDeLaPublicacion = visible.Publicacion.CreadoPorUsuarioId == usuarioId;
        var lista = await comentarios.ObtenerConAutorAsync(publicacionId, pagina, tamanoPagina, cancellationToken);
        return lista.Select(c => new ComentarioVista(
            c, c.PerfilAutor, c.PerfilAutor.FotoArchivo?.ClaveObjeto,
            esAutorDeLaPublicacion || c.CreadoPorUsuarioId == usuarioId)).ToList();
    }

    /// <summary>null si la publicación no se puede ver.</summary>
    public async Task<ComentarioVista?> ComentarAsync(Guid usuarioId, Guid publicacionId, ComentarDto entrada, CancellationToken cancellationToken)
    {
        var contenido = entrada.Contenido.Trim();
        if (contenido.Length == 0) throw new ExcepcionDominio("Escribe un comentario.");

        var visible = await feed.ObtenerAsync(publicacionId, usuarioId, cancellationToken);
        var autor = await miPerfil.ObtenerAsync(usuarioId, cancellationToken);
        if (visible is null || autor is null) return null;
        if (!visible.Publicacion.ComentariosHabilitados)
            throw new ExcepcionDominio("Los comentarios están desactivados en esta publicación.");

        var comentario = new Comentario
        {
            PublicacionId = publicacionId,
            PerfilAutorId = autor.Perfil.Id,
            CreadoPorUsuarioId = usuarioId,
            Contenido = contenido
        };
        await unitOfWork.EjecutarEnTransaccionAsync(async ct =>
        {
            await comentarios.AgregarAsync(comentario, ct);
            await publicaciones.AjustarContadoresAsync(publicacionId, 0, 1, ct);
            return true;
        }, cancellationToken);

        return new ComentarioVista(comentario, autor.Perfil, autor.ClaveFoto, true);
    }

    /// <summary>false si no existe; ComentarioAjenoException si no es suyo ni de su publicación.</summary>
    public async Task<bool> EliminarComentarioAsync(Guid usuarioId, Guid publicacionId, Guid comentarioId, CancellationToken cancellationToken)
    {
        var comentario = await comentarios.ObtenerPorClaveAsync([comentarioId], cancellationToken);
        if (comentario is null || comentario.PublicacionId != publicacionId) return false;
        var publicacion = await publicaciones.ObtenerPorClaveAsync([publicacionId], cancellationToken);
        if (publicacion is null) return false;
        if (comentario.CreadoPorUsuarioId != usuarioId && publicacion.CreadoPorUsuarioId != usuarioId)
            throw new ComentarioAjenoException();

        await unitOfWork.EjecutarEnTransaccionAsync(async ct =>
        {
            // Borrado lógico: el contexto convierte el DELETE en eliminado_en = ahora.
            await comentarios.EliminarAsync(comentario, ct);
            await publicaciones.AjustarContadoresAsync(publicacionId, 0, -1, ct);
            return true;
        }, cancellationToken);
        return true;
    }

    /// <summary>null si no hay un perfil activo con ese @usuario.</summary>
    public async Task<PerfilVista?> ObtenerPerfilAsync(Guid usuarioId, string nombreUsuario, CancellationToken cancellationToken)
    {
        var perfil = await perfiles.ObtenerActivoPorNombreUsuarioAsync(Normalizar(nombreUsuario), cancellationToken);
        if (perfil is null) return null;

        var esMio = perfil.UsuarioId == usuarioId;
        var loSigo = !esMio && (await seguimientos.ObtenerPerfilesSeguidosAsync(usuarioId, [perfil.Id], cancellationToken)).Count > 0;
        return new PerfilVista(perfil, perfil.FotoArchivo?.ClaveObjeto, esMio, loSigo);
    }

    /// <summary>Publicaciones públicas del perfil; null si el perfil no existe.</summary>
    public async Task<IReadOnlyList<PublicacionFeed>?> ListarPublicacionesDePerfilAsync(Guid usuarioId, string nombreUsuario, int pagina, int tamanoPagina, CancellationToken cancellationToken)
    {
        var perfil = await perfiles.ObtenerActivoPorNombreUsuarioAsync(Normalizar(nombreUsuario), cancellationToken);
        if (perfil is null) return null;
        return await feed.ListarAsync(usuarioId, pagina, tamanoPagina, cancellationToken, perfilAutorId: perfil.Id);
    }

    /// <summary>Perfiles activos para descubrir, con la marca de si ya los sigue.</summary>
    public async Task<IReadOnlyList<PerfilVista>> SugerenciasAsync(Guid usuarioId, int cantidad, CancellationToken cancellationToken)
    {
        var candidatos = await perfiles.ObtenerSugeridosAsync(usuarioId, cantidad, cancellationToken);
        var seguidos = (await seguimientos.ObtenerPerfilesSeguidosAsync(usuarioId, candidatos.Select(p => p.Id).ToList(), cancellationToken)).ToHashSet();
        return candidatos.Select(p => new PerfilVista(p, p.FotoArchivo?.ClaveObjeto, false, seguidos.Contains(p.Id))).ToList();
    }

    /// <summary>Sigue o deja de seguir. Repetir la misma acción no cambia nada. null si el perfil no existe.</summary>
    public async Task<ResultadoSeguimiento?> SeguirAsync(Guid usuarioId, string nombreUsuario, bool seguir, CancellationToken cancellationToken)
    {
        var perfil = await perfiles.ObtenerActivoPorNombreUsuarioAsync(Normalizar(nombreUsuario), cancellationToken);
        var propio = await miPerfil.ObtenerAsync(usuarioId, cancellationToken);
        if (perfil is null || propio is null) return null;
        if (perfil.UsuarioId == usuarioId) throw new ExcepcionDominio("No puedes seguir tu propio perfil.");

        var existente = await seguimientos.ObtenerPorClaveAsync([usuarioId, perfil.Id], cancellationToken);
        if ((existente is not null) == seguir) return new ResultadoSeguimiento(seguir, perfil.TotalSeguidores);

        var cambio = seguir ? 1 : -1;
        await unitOfWork.EjecutarEnTransaccionAsync(async ct =>
        {
            if (seguir)
                await seguimientos.AgregarAsync(new Seguimiento
                {
                    UsuarioSeguidorId = usuarioId,
                    PerfilSeguidoId = perfil.Id,
                    Estado = EstadoSeguimiento.Activo,
                    AceptadoEn = DateTime.UtcNow
                }, ct);
            else
                await seguimientos.EliminarAsync(existente!, ct);

            await perfiles.AjustarSeguimientoAsync(perfil.Id, cambio, 0, ct);
            await perfiles.AjustarSeguimientoAsync(propio.Perfil.Id, 0, cambio, ct);
            return true;
        }, cancellationToken);

        return new ResultadoSeguimiento(seguir, Math.Max(0, perfil.TotalSeguidores + cambio));
    }

    private static string Normalizar(string nombreUsuario) => nombreUsuario.Trim().TrimStart('@').ToLowerInvariant();
}
