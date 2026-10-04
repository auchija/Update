using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de oportunidades.</summary>
public sealed class OportunidadRepository(ApplicationDbContext contexto) : GenericRepository<Oportunidad>(contexto), IOportunidadRepository
{
    public async Task<IReadOnlyList<Oportunidad>> ObtenerPorPerfilAutorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.PerfilAutorId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<Oportunidad>> ObtenerPorEstadoAsync(EstadoOportunidad valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
