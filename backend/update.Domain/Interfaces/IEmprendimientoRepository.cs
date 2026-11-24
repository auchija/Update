using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IEmprendimientoRepository : IRepository<Emprendimiento>
{
    Task<IReadOnlyList<Emprendimiento>> ObtenerPorCategoriaIdAsync(int valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
