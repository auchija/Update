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
}
