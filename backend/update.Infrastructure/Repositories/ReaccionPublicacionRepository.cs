using Microsoft.EntityFrameworkCore;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de reacciones_publicacion.</summary>
public sealed class ReaccionPublicacionRepository(ApplicationDbContext contexto) : GenericRepository<ReaccionPublicacion>(contexto), IReaccionPublicacionRepository
{
    public async Task<IReadOnlyList<ReaccionPublicacion>> ObtenerPorPublicacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.PublicacionId == valor, pagina, tamanoPagina, cancellationToken);

    public async Task<IReadOnlyList<Guid>> ObtenerPublicacionesConReaccionAsync(Guid perfilId, IReadOnlyCollection<Guid> publicacionIds, CancellationToken cancellationToken = default)
        => publicacionIds.Count == 0 ? [] : await Conjunto.AsNoTracking()
            .Where(e => e.PerfilId == perfilId && publicacionIds.Contains(e.PublicacionId))
            .Select(e => e.PublicacionId)
            .ToListAsync(cancellationToken);
}
