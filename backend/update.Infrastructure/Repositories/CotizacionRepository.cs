using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de cotizaciones.</summary>
public sealed class CotizacionRepository(ApplicationDbContext contexto) : GenericRepository<Cotizacion>(contexto), ICotizacionRepository
{
    public async Task<IReadOnlyList<Cotizacion>> ObtenerPorSolicitudCotizacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.SolicitudCotizacionId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<Cotizacion>> ObtenerPorEstadoAsync(EstadoCotizacion valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
