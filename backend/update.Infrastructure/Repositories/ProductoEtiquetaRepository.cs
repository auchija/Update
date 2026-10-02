using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de productos_etiquetas.</summary>
public sealed class ProductoEtiquetaRepository(ApplicationDbContext contexto) : GenericRepository<ProductoEtiqueta>(contexto), IProductoEtiquetaRepository
{
    public async Task<IReadOnlyList<ProductoEtiqueta>> ObtenerPorProductoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.ProductoId == valor, pagina, tamanoPagina, cancellationToken);
}
