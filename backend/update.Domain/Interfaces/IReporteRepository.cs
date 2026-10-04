using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de reportes.</summary>
public interface IReporteRepository : IRepository<Reporte>
{
    Task<IReadOnlyList<Reporte>> ObtenerPorMotivoCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Reporte>> ObtenerPorEstadoAsync(EstadoReporte valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
