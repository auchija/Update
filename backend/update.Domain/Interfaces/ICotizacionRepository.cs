using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de cotizaciones.</summary>
public interface ICotizacionRepository : IRepository<Cotizacion>
{
    Task<IReadOnlyList<Cotizacion>> ObtenerPorSolicitudCotizacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Cotizacion>> ObtenerPorEstadoAsync(EstadoCotizacion valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
