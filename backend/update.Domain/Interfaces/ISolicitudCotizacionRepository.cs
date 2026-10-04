using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de solicitudes_cotizacion.</summary>
public interface ISolicitudCotizacionRepository : IRepository<SolicitudCotizacion>
{
    Task<IReadOnlyList<SolicitudCotizacion>> ObtenerPorEmprendimientoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SolicitudCotizacion>> ObtenerPorEstadoAsync(EstadoSolicitud valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
