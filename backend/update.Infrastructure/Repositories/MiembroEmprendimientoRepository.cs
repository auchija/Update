using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

public sealed class MiembroEmprendimientoRepository(ApplicationDbContext contexto) : GenericRepository<MiembroEmprendimiento>(contexto), IMiembroEmprendimientoRepository
{
    public async Task<IReadOnlyList<MiembroEmprendimiento>> ObtenerPorEmprendimientoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.EmprendimientoId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<MiembroEmprendimiento>> ObtenerPorEstadoAsync(EstadoMiembro valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
