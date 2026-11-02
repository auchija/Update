using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de publicaciones.</summary>
public interface IPublicacionRepository : IRepository<Publicacion>
{
    Task<IReadOnlyList<Publicacion>> ObtenerFeedAsync(Guid usuarioId, int pagina, int tamanoPagina, CancellationToken cancellationToken = default);
    Task<Publicacion?> ObtenerConAutorAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Publicacion>> ObtenerPorPerfilAutorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
