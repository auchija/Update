using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IMotivoReporteRepository : IRepository<MotivoReporte>
{
    Task<IReadOnlyList<MotivoReporte>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
