using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de comentarios.</summary>
public sealed class ComentarioRepository(ApplicationDbContext contexto) : GenericRepository<Comentario>(contexto), IComentarioRepository
{
    public async Task<IReadOnlyList<Comentario>> ObtenerPorPublicacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.PublicacionId == valor, pagina, tamanoPagina, cancellationToken);
}
