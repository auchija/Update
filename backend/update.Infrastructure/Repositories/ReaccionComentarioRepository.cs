using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de reacciones_comentario.</summary>
public sealed class ReaccionComentarioRepository(ApplicationDbContext contexto) : GenericRepository<ReaccionComentario>(contexto), IReaccionComentarioRepository
{
    public async Task<IReadOnlyList<ReaccionComentario>> ObtenerPorComentarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.ComentarioId == valor, pagina, tamanoPagina, cancellationToken);
}
