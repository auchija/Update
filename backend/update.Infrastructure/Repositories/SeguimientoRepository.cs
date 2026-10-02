using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de seguimientos.</summary>
public sealed class SeguimientoRepository(ApplicationDbContext contexto) : GenericRepository<Seguimiento>(contexto), ISeguimientoRepository
{
    public async Task<IReadOnlyList<Seguimiento>> ObtenerPorUsuarioSeguidorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.UsuarioSeguidorId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<Seguimiento>> ObtenerPorEstadoAsync(EstadoSeguimiento valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
