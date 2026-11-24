using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IMencionRepository : IRepository<Mencion>
{
    Task<IReadOnlyList<Mencion>> ObtenerPorPublicacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
