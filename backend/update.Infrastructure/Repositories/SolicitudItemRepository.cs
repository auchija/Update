using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de solicitud_items.</summary>
public sealed class SolicitudItemRepository(ApplicationDbContext contexto) : GenericRepository<SolicitudItem>(contexto), ISolicitudItemRepository
{
    public async Task<IReadOnlyList<SolicitudItem>> ObtenerPorSolicitudCotizacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.SolicitudCotizacionId == valor, pagina, tamanoPagina, cancellationToken);
}
