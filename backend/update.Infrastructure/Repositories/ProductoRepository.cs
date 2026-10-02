using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de productos.</summary>
public sealed class ProductoRepository(ApplicationDbContext contexto) : GenericRepository<Producto>(contexto), IProductoRepository
{
    public async Task<IReadOnlyList<Producto>> ObtenerPorEmprendimientoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.EmprendimientoId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<Producto>> ObtenerPorEstadoAsync(EstadoProducto valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
