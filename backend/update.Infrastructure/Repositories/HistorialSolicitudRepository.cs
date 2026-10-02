using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de historial_solicitudes.</summary>
public sealed class HistorialSolicitudRepository(ApplicationDbContext contexto) : GenericRepository<HistorialSolicitud>(contexto), IHistorialSolicitudRepository
{
    public async Task<IReadOnlyList<HistorialSolicitud>> ObtenerPorSolicitudCotizacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.SolicitudCotizacionId == valor, pagina, tamanoPagina, cancellationToken);
}
