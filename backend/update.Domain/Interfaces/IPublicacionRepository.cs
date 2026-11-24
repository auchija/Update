using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IPublicacionRepository : IRepository<Publicacion>
{
    Task<IReadOnlyList<Publicacion>> ObtenerPorPerfilAutorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Publicacion>> ObtenerRecientesAsync(int pagina, int tamanoPagina, CancellationToken cancellationToken = default, Guid? usuarioId = null, Guid? perfilAutorId = null, Guid? visorUsuarioId = null);
    Task AjustarContadoresAsync(Guid publicacionId, int reacciones, int comentarios, CancellationToken cancellationToken = default);
}
