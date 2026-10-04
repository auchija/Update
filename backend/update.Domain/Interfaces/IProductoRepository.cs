using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de productos.</summary>
public interface IProductoRepository : IRepository<Producto>
{
    Task<IReadOnlyList<Producto>> ObtenerPorEmprendimientoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Producto>> ObtenerPorEstadoAsync(EstadoProducto valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
