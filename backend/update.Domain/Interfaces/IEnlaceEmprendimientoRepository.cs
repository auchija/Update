using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IEnlaceEmprendimientoRepository : IRepository<EnlaceEmprendimiento>
{
    Task<IReadOnlyList<EnlaceEmprendimiento>> ObtenerPorEmprendimientoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
