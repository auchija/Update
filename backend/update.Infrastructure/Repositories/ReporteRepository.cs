using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de reportes.</summary>
public sealed class ReporteRepository(ApplicationDbContext contexto) : GenericRepository<Reporte>(contexto), IReporteRepository
{
    public async Task<IReadOnlyList<Reporte>> ObtenerPorMotivoCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.MotivoCodigo == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<Reporte>> ObtenerPorEstadoAsync(EstadoReporte valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
