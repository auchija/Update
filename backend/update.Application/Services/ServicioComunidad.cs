using update.Application.DTOs;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;

namespace update.Application.Services;

public sealed record ComentarioVista(Comentario Comentario, Perfil Autor, string? ClaveFotoAutor, bool PuedeEliminar);
public sealed record PerfilVista(Perfil Perfil, string? ClaveFoto, bool EsMio, bool LoSigo, bool SolicitudPendiente, DateTime? Fecha = null)
{
    public bool PuedeVerContenido => !Perfil.EsPrivado || EsMio || LoSigo;
}
public sealed record ResultadoMeGusta(bool MeGusta, int TotalReacciones);
public sealed record ResultadoSeguimiento(bool LoSigo, bool SolicitudPendiente, int TotalSeguidores);

public sealed class ComentarioAjenoException() : Exception("Solo puedes borrar tus comentarios o los de tus publicaciones.");
public sealed class PerfilPrivadoException() : Exception("Esta cuenta es privada. Síguela para ver su contenido.");

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
            await comentarios.EliminarAsync(comentario, ct);
            await publicaciones.AjustarContadoresAsync(publicacionId, 0, -1, ct);
            return true;
        }, cancellationToken);
        return true;
    }

    public async Task<PerfilVista?> ObtenerPerfilAsync(Guid usuarioId, string nombreUsuario, CancellationToken cancellationToken)
    {
        var perfil = await perfiles.ObtenerActivoPorNombreUsuarioAsync(Normalizar(nombreUsuario), cancellationToken);
        if (perfil is null) return null;
        return (await VistasAsync(usuarioId, [new PerfilRelacion(perfil, perfil.FotoArchivo?.ClaveObjeto, perfil.CreadoEn)], cancellationToken))[0];
    }

    public async Task<IReadOnlyList<PublicacionFeed>?> ListarPublicacionesDePerfilAsync(Guid usuarioId, string nombreUsuario, int pagina, int tamanoPagina, CancellationToken cancellationToken)
    {
        var vista = await ObtenerPerfilAsync(usuarioId, nombreUsuario, cancellationToken);
        if (vista is null) return null;
        if (!vista.PuedeVerContenido) return [];
        return await feed.ListarAsync(usuarioId, pagina, tamanoPagina, cancellationToken, perfilAutorId: vista.Perfil.Id);
    }

    public async Task<IReadOnlyList<PerfilVista>?> ListarSeguidoresAsync(Guid usuarioId, string nombreUsuario, int pagina, int tamanoPagina, CancellationToken cancellationToken)
    {
        var vista = await ObtenerPerfilVisibleAsync(usuarioId, nombreUsuario, cancellationToken);
        if (vista is null) return null;
        var lista = await seguimientos.ObtenerSeguidoresAsync(vista.Perfil.Id, EstadoSeguimiento.Activo, pagina, tamanoPagina, cancellationToken);
        return await VistasAsync(usuarioId, lista, cancellationToken);
    }

    public async Task<IReadOnlyList<PerfilVista>?> ListarSeguidosAsync(Guid usuarioId, string nombreUsuario, int pagina, int tamanoPagina, CancellationToken cancellationToken)
    {
        var vista = await ObtenerPerfilVisibleAsync(usuarioId, nombreUsuario, cancellationToken);
        if (vista is null) return null;
        var lista = await seguimientos.ObtenerSeguidosAsync(vista.Perfil.UsuarioId, pagina, tamanoPagina, cancellationToken);
        return await VistasAsync(usuarioId, lista, cancellationToken);
    }

    public async Task<IReadOnlyList<PerfilVista>?> RelacionadosAsync(Guid usuarioId, string nombreUsuario, int cantidad, CancellationToken cancellationToken)
    {
        var vista = await ObtenerPerfilAsync(usuarioId, nombreUsuario, cancellationToken);
        if (vista is null) return null;
        var perfil = vista.Perfil;
        cantidad = Math.Clamp(cantidad, 1, 20);

        var lista = vista.PuedeVerContenido
            ? (await seguimientos.ObtenerRelacionadosAsync(perfil.Id, usuarioId, cantidad, cancellationToken)).ToList()
            : [];
        if (lista.Count < cantidad)
        {
            var yaEstan = lista.Select(r => r.Perfil.Id).Append(perfil.Id).ToHashSet();
            var extra = await perfiles.ObtenerSugeridosAsync(usuarioId, cantidad + lista.Count + 1, cancellationToken);
            lista.AddRange(extra
                .Where(p => !yaEstan.Contains(p.Id))
                .Take(cantidad - lista.Count)
                .Select(p => new PerfilRelacion(p, p.FotoArchivo?.ClaveObjeto, p.CreadoEn)));
        }
        return await VistasAsync(usuarioId, lista, cancellationToken);
    }

    public async Task<IReadOnlyList<PerfilVista>> SugerenciasAsync(Guid usuarioId, int cantidad, CancellationToken cancellationToken)
    {
        var candidatos = await perfiles.ObtenerSugeridosAsync(usuarioId, cantidad, cancellationToken);
        return await VistasAsync(usuarioId, candidatos.Select(p => new PerfilRelacion(p, p.FotoArchivo?.ClaveObjeto, p.CreadoEn)).ToList(), cancellationToken);
    }

    public async Task<ResultadoSeguimiento?> SeguirAsync(Guid usuarioId, string nombreUsuario, bool seguir, CancellationToken cancellationToken)
    {
        var perfil = await perfiles.ObtenerActivoPorNombreUsuarioAsync(Normalizar(nombreUsuario), cancellationToken);
        var propio = await miPerfil.ObtenerAsync(usuarioId, cancellationToken);
        if (perfil is null || propio is null) return null;
        if (perfil.UsuarioId == usuarioId) throw new ExcepcionDominio("No puedes seguir tu propio perfil.");

        var existente = await seguimientos.ObtenerPorClaveAsync([usuarioId, perfil.Id], cancellationToken);
        if (seguir && existente is not null)
            return Resultado(existente.Estado, perfil.TotalSeguidores);
        if (!seguir && existente is null)
            return new ResultadoSeguimiento(false, false, perfil.TotalSeguidores);

        var estadoNuevo = perfil.EsPrivado ? EstadoSeguimiento.Pendiente : EstadoSeguimiento.Activo;
        var cambio = seguir
            ? (estadoNuevo == EstadoSeguimiento.Activo ? 1 : 0)
            : (existente!.Estado == EstadoSeguimiento.Activo ? -1 : 0);

        await unitOfWork.EjecutarEnTransaccionAsync(async ct =>
        {
            if (seguir)
                await seguimientos.AgregarAsync(new Seguimiento
                {
                    UsuarioSeguidorId = usuarioId,
                    PerfilSeguidoId = perfil.Id,
                    Estado = estadoNuevo,
                    AceptadoEn = estadoNuevo == EstadoSeguimiento.Activo ? DateTime.UtcNow : null
                }, ct);
            else
                await seguimientos.EliminarAsync(existente!, ct);

            if (cambio != 0)
            {
                await perfiles.AjustarSeguimientoAsync(perfil.Id, cambio, 0, ct);
                await perfiles.AjustarSeguimientoAsync(propio.Perfil.Id, 0, cambio, ct);
            }
            return true;
        }, cancellationToken);

        var total = Math.Max(0, perfil.TotalSeguidores + cambio);
        return seguir ? Resultado(estadoNuevo, total) : new ResultadoSeguimiento(false, false, total);
    }

    public async Task<IReadOnlyList<PerfilVista>> ListarSolicitudesAsync(Guid usuarioId, int pagina, int tamanoPagina, CancellationToken cancellationToken)
    {
        var propio = await miPerfil.ObtenerAsync(usuarioId, cancellationToken);
        if (propio is null) return [];
        var lista = await seguimientos.ObtenerSeguidoresAsync(propio.Perfil.Id, EstadoSeguimiento.Pendiente, pagina, tamanoPagina, cancellationToken);
        return await VistasAsync(usuarioId, lista, cancellationToken);
    }

    public async Task<bool> ResponderSolicitudAsync(Guid usuarioId, string nombreUsuarioSolicitante, bool aceptar, CancellationToken cancellationToken)
    {
        var propio = await miPerfil.ObtenerAsync(usuarioId, cancellationToken);
        var solicitante = await perfiles.ObtenerActivoPorNombreUsuarioAsync(Normalizar(nombreUsuarioSolicitante), cancellationToken);
        if (propio is null || solicitante is null) return false;

        var solicitud = await seguimientos.ObtenerPorClaveAsync([solicitante.UsuarioId, propio.Perfil.Id], cancellationToken);
        if (solicitud is not { Estado: EstadoSeguimiento.Pendiente }) return false;

        var perfilSolicitante = (await perfiles.BuscarAsync(
            p => p.UsuarioId == solicitante.UsuarioId && p.Tipo == TipoPerfil.Persona, 1, 1, cancellationToken)).FirstOrDefault() ?? solicitante;

        await unitOfWork.EjecutarEnTransaccionAsync(async ct =>
        {
            if (aceptar)
            {
                solicitud.Aceptar();
                await seguimientos.ActualizarAsync(solicitud, ct);
                await perfiles.AjustarSeguimientoAsync(propio.Perfil.Id, 1, 0, ct);
                await perfiles.AjustarSeguimientoAsync(perfilSolicitante.Id, 0, 1, ct);
            }
            else
            {
                await seguimientos.EliminarAsync(solicitud, ct);
            }
            return true;
        }, cancellationToken);
        return true;
    }

    private async Task<PerfilVista?> ObtenerPerfilVisibleAsync(Guid usuarioId, string nombreUsuario, CancellationToken cancellationToken)
    {
        var vista = await ObtenerPerfilAsync(usuarioId, nombreUsuario, cancellationToken);
        if (vista is not null && !vista.PuedeVerContenido) throw new PerfilPrivadoException();
        return vista;
    }

    private async Task<IReadOnlyList<PerfilVista>> VistasAsync(Guid usuarioId, IReadOnlyList<PerfilRelacion> lista, CancellationToken cancellationToken)
    {
        var estados = await seguimientos.ObtenerEstadosAsync(usuarioId, lista.Select(r => r.Perfil.Id).Distinct().ToList(), cancellationToken);
        return lista.Select(r =>
        {
            var estado = estados.TryGetValue(r.Perfil.Id, out var e) ? e : (EstadoSeguimiento?)null;
            return new PerfilVista(
                r.Perfil, r.ClaveFoto, r.Perfil.UsuarioId == usuarioId,
                estado == EstadoSeguimiento.Activo, estado == EstadoSeguimiento.Pendiente, r.Fecha);
        }).ToList();
    }

    private static ResultadoSeguimiento Resultado(EstadoSeguimiento estado, int totalSeguidores)
        => new(estado == EstadoSeguimiento.Activo, estado == EstadoSeguimiento.Pendiente, totalSeguidores);

    private static string Normalizar(string nombreUsuario) => nombreUsuario.Trim().TrimStart('@').ToLowerInvariant();
}
