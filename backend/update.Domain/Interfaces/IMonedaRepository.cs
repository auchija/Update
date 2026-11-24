using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IMonedaRepository : IRepository<Moneda>
{
    Task<IReadOnlyList<Moneda>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
