using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IBloqueoRepository : IRepository<Bloqueo>
{
    Task<IReadOnlyList<Bloqueo>> ObtenerPorUsuarioBloqueadorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
