using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IProductoArchivoRepository : IRepository<ProductoArchivo>
{
    Task<IReadOnlyList<ProductoArchivo>> ObtenerPorProductoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
