using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de motivos_reporte.</summary>
public interface IMotivoReporteRepository : IRepository<MotivoReporte>
{
    Task<IReadOnlyList<MotivoReporte>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
