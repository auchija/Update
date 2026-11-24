using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IMiembroEmprendimientoRepository : IRepository<MiembroEmprendimiento>
{
    Task<IReadOnlyList<MiembroEmprendimiento>> ObtenerPorEmprendimientoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MiembroEmprendimiento>> ObtenerPorEstadoAsync(EstadoMiembro valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
