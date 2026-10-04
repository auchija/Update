using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de productos_archivos.</summary>
public sealed class ProductoArchivoRepository(ApplicationDbContext contexto) : GenericRepository<ProductoArchivo>(contexto), IProductoArchivoRepository
{
    public async Task<IReadOnlyList<ProductoArchivo>> ObtenerPorProductoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.ProductoId == valor, pagina, tamanoPagina, cancellationToken);
}
