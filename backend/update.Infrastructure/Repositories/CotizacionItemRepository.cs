using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de cotizacion_items.</summary>
public sealed class CotizacionItemRepository(ApplicationDbContext contexto) : GenericRepository<CotizacionItem>(contexto), ICotizacionItemRepository
{
    public async Task<IReadOnlyList<CotizacionItem>> ObtenerPorCotizacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.CotizacionId == valor, pagina, tamanoPagina, cancellationToken);
}
