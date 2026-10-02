using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de resenas.</summary>
public sealed class ResenaRepository(ApplicationDbContext contexto) : GenericRepository<Resena>(contexto), IResenaRepository
{
    public async Task<IReadOnlyList<Resena>> ObtenerPorUsuarioResenadorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.UsuarioResenadorId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<Resena>> ObtenerPorEstadoAsync(EstadoResena valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
