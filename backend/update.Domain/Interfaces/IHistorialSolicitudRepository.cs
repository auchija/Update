using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de historial_solicitudes.</summary>
public interface IHistorialSolicitudRepository : IRepository<HistorialSolicitud>
{
    Task<IReadOnlyList<HistorialSolicitud>> ObtenerPorSolicitudCotizacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
