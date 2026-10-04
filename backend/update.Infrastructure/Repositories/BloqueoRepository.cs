using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de bloqueos.</summary>
public sealed class BloqueoRepository(ApplicationDbContext contexto) : GenericRepository<Bloqueo>(contexto), IBloqueoRepository
{
    public async Task<IReadOnlyList<Bloqueo>> ObtenerPorUsuarioBloqueadorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.UsuarioBloqueadorId == valor, pagina, tamanoPagina, cancellationToken);
}
