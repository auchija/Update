using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de comentarios.</summary>
public interface IComentarioRepository : IRepository<Comentario>
{
    Task<IReadOnlyList<Comentario>> ObtenerPorPublicacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
