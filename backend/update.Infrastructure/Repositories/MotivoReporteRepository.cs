using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de motivos_reporte.</summary>
public sealed class MotivoReporteRepository(ApplicationDbContext contexto) : GenericRepository<MotivoReporte>(contexto), IMotivoReporteRepository
{
    public async Task<IReadOnlyList<MotivoReporte>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Codigo == valor, pagina, tamanoPagina, cancellationToken);
}
