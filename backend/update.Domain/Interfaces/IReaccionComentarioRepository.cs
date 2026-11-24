using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IReaccionComentarioRepository : IRepository<ReaccionComentario>
{
    Task<IReadOnlyList<ReaccionComentario>> ObtenerPorComentarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
