using Microsoft.EntityFrameworkCore;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

public sealed class SeguimientoRepository(ApplicationDbContext contexto) : GenericRepository<Seguimiento>(contexto), ISeguimientoRepository
{
    public async Task<IReadOnlyList<Seguimiento>> ObtenerPorUsuarioSeguidorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.UsuarioSeguidorId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<Seguimiento>> ObtenerPorEstadoAsync(EstadoSeguimiento valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);

    public async Task<IReadOnlyDictionary<Guid, EstadoSeguimiento>> ObtenerEstadosAsync(Guid usuarioId, IReadOnlyCollection<Guid> perfilIds, CancellationToken cancellationToken = default)
    {
        if (perfilIds.Count == 0) return new Dictionary<Guid, EstadoSeguimiento>();
        return await Conjunto.AsNoTracking()
            .Where(e => e.UsuarioSeguidorId == usuarioId && perfilIds.Contains(e.PerfilSeguidoId))
            .ToDictionaryAsync(e => e.PerfilSeguidoId, e => e.Estado, cancellationToken);
    }

    public async Task<IReadOnlyList<PerfilRelacion>> ObtenerSeguidoresAsync(Guid perfilId, EstadoSeguimiento estado, int pagina, int tamanoPagina, CancellationToken cancellationToken = default)
    {
        ValidarPagina(pagina, tamanoPagina);
        return await Conjunto.AsNoTracking()
            .Where(s => s.PerfilSeguidoId == perfilId && s.Estado == estado)
            .Join(PerfilesVisibles(), s => s.UsuarioSeguidorId, p => p.UsuarioId, (s, p) => new { s, p })
            .OrderByDescending(x => x.s.CreadoEn).ThenBy(x => x.p.Id)
            .Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina)
            .Select(x => new PerfilRelacion(x.p, x.p.FotoArchivo != null ? x.p.FotoArchivo.ClaveObjeto : null, x.s.CreadoEn))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PerfilRelacion>> ObtenerSeguidosAsync(Guid usuarioId, int pagina, int tamanoPagina, CancellationToken cancellationToken = default)
    {
        ValidarPagina(pagina, tamanoPagina);
        return await Conjunto.AsNoTracking()
            .Where(s => s.UsuarioSeguidorId == usuarioId && s.Estado == EstadoSeguimiento.Activo)
            .Join(Contexto.Set<Perfil>().Where(p => p.Estado == EstadoCuenta.Activo), s => s.PerfilSeguidoId, p => p.Id, (s, p) => new { s, p })
            .OrderByDescending(x => x.s.CreadoEn).ThenBy(x => x.p.Id)
            .Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina)
            .Select(x => new PerfilRelacion(x.p, x.p.FotoArchivo != null ? x.p.FotoArchivo.ClaveObjeto : null, x.s.CreadoEn))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PerfilRelacion>> ObtenerRelacionadosAsync(Guid perfilId, Guid excluirUsuarioId, int cantidad, CancellationToken cancellationToken = default)
    {
        cantidad = Math.Clamp(cantidad, 1, 30);
        var seguidores = Conjunto
            .Where(s => s.PerfilSeguidoId == perfilId && s.Estado == EstadoSeguimiento.Activo)
            .Select(s => s.UsuarioSeguidorId);

        var conteos = await Conjunto.AsNoTracking()
            .Where(s => seguidores.Contains(s.UsuarioSeguidorId) && s.Estado == EstadoSeguimiento.Activo && s.PerfilSeguidoId != perfilId)
            .GroupBy(s => s.PerfilSeguidoId)
            .Select(g => new { PerfilId = g.Key, Total = g.Count() })
            .OrderByDescending(x => x.Total).ThenBy(x => x.PerfilId)
            .Take(cantidad + 5)
            .ToListAsync(cancellationToken);
        if (conteos.Count == 0) return [];

        var ids = conteos.Select(x => x.PerfilId).ToList();
        var perfiles = await PerfilesVisibles().AsNoTracking()
            .Where(p => ids.Contains(p.Id) && p.UsuarioId != excluirUsuarioId)
            .Select(p => new PerfilRelacion(p, p.FotoArchivo != null ? p.FotoArchivo.ClaveObjeto : null, p.CreadoEn))
            .ToListAsync(cancellationToken);

        var orden = ids.Select((id, i) => (id, i)).ToDictionary(x => x.id, x => x.i);
        return perfiles.OrderBy(r => orden[r.Perfil.Id]).Take(cantidad).ToList();
    }

    private IQueryable<Perfil> PerfilesVisibles()
        => Contexto.Set<Perfil>().Where(p => p.Tipo == TipoPerfil.Persona && p.Estado == EstadoCuenta.Activo);

    private static void ValidarPagina(int pagina, int tamanoPagina)
    {
        if (pagina < 1 || pagina > 100000 || tamanoPagina < 1 || tamanoPagina > 100)
            throw new ExcepcionDominio("La página debe estar entre 1 y 100000; el tamaño, entre 1 y 100.");
    }
}
