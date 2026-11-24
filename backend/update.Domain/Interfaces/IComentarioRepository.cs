using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IComentarioRepository : IRepository<Comentario>
{
    Task<IReadOnlyList<Comentario>> ObtenerPorPublicacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Comentario>> ObtenerConAutorAsync(Guid publicacionId, int pagina, int tamanoPagina, CancellationToken cancellationToken = default);
}
