using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de cotizacion_items.</summary>
public interface ICotizacionItemRepository : IRepository<CotizacionItem>
{
    Task<IReadOnlyList<CotizacionItem>> ObtenerPorCotizacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
