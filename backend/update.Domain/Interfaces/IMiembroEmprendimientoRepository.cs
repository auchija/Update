using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de miembros_emprendimiento.</summary>
public interface IMiembroEmprendimientoRepository : IRepository<MiembroEmprendimiento>
{
    Task<IReadOnlyList<MiembroEmprendimiento>> ObtenerPorEmprendimientoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MiembroEmprendimiento>> ObtenerPorEstadoAsync(EstadoMiembro valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
