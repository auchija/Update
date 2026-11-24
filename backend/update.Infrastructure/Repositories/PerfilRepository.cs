using Microsoft.EntityFrameworkCore;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

public sealed class PerfilRepository(ApplicationDbContext contexto) : GenericRepository<Perfil>(contexto), IPerfilRepository
{
    public async Task<IReadOnlyList<Perfil>> ObtenerPorCiudadIdAsync(int? valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.CiudadId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<Perfil>> ObtenerPorEstadoAsync(EstadoCuenta valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);

    public async Task<Perfil?> ObtenerActivoPorNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken = default)
        => await Conjunto.AsNoTracking()
            .Include(e => e.FotoArchivo)
            .FirstOrDefaultAsync(e => e.NombreUsuario == nombreUsuario && e.Estado == EstadoCuenta.Activo, cancellationToken);

    public async Task<IReadOnlyList<Perfil>> ObtenerSugeridosAsync(Guid excluirUsuarioId, int cantidad, CancellationToken cancellationToken = default)
        => await Conjunto.AsNoTracking()
            .Include(e => e.FotoArchivo)
            .Where(e => e.UsuarioId != excluirUsuarioId && e.Tipo == TipoPerfil.Persona && e.Estado == EstadoCuenta.Activo
                && e.TotalPublicaciones > 0)
            .OrderByDescending(e => e.TotalPublicaciones).ThenByDescending(e => e.TotalSeguidores).ThenBy(e => e.Id)
            .Take(Math.Clamp(cantidad, 1, 30))
            .ToListAsync(cancellationToken);

    public async Task AjustarSeguimientoAsync(Guid perfilId, int seguidores, int seguidos, CancellationToken cancellationToken = default)
        => await Conjunto.Where(e => e.Id == perfilId).ExecuteUpdateAsync(s => s
            .SetProperty(e => e.TotalSeguidores, e => Math.Max(0, e.TotalSeguidores + seguidores))
            .SetProperty(e => e.TotalSeguidos, e => Math.Max(0, e.TotalSeguidos + seguidos)), cancellationToken);

    public async Task AjustarPublicacionesAsync(Guid perfilId, int cambio, CancellationToken cancellationToken = default)
        => await Conjunto.Where(e => e.Id == perfilId).ExecuteUpdateAsync(s => s
            .SetProperty(e => e.TotalPublicaciones, e => Math.Max(0, e.TotalPublicaciones + cambio)), cancellationToken);

    public async Task ActualizarDatosAsync(Perfil perfil, CancellationToken cancellationToken = default)
    {
        perfil.Validar();
        var entrada = Contexto.Entry(perfil);
        entrada.State = EntityState.Modified;
        entrada.Property(e => e.TotalSeguidores).IsModified = false;
        entrada.Property(e => e.TotalSeguidos).IsModified = false;
        entrada.Property(e => e.TotalPublicaciones).IsModified = false;
        await Contexto.SaveChangesAsync(cancellationToken);
    }
}
