using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de reacciones_comentario.</summary>
public interface IReaccionComentarioRepository : IRepository<ReaccionComentario>
{
    Task<IReadOnlyList<ReaccionComentario>> ObtenerPorComentarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
