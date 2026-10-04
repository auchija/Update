using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de solicitud_items.</summary>
public interface ISolicitudItemRepository : IRepository<SolicitudItem>
{
    Task<IReadOnlyList<SolicitudItem>> ObtenerPorSolicitudCotizacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
