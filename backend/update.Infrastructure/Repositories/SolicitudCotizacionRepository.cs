using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de solicitudes_cotizacion.</summary>
public sealed class SolicitudCotizacionRepository(ApplicationDbContext contexto) : GenericRepository<SolicitudCotizacion>(contexto), ISolicitudCotizacionRepository
{
    public async Task<IReadOnlyList<SolicitudCotizacion>> ObtenerPorEmprendimientoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.EmprendimientoId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<SolicitudCotizacion>> ObtenerPorEstadoAsync(EstadoSolicitud valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
