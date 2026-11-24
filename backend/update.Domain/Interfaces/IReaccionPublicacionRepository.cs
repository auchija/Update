using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IReaccionPublicacionRepository : IRepository<ReaccionPublicacion>
{
    Task<IReadOnlyList<ReaccionPublicacion>> ObtenerPorPublicacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Guid>> ObtenerPublicacionesConReaccionAsync(Guid perfilId, IReadOnlyCollection<Guid> publicacionIds, CancellationToken cancellationToken = default);
}
