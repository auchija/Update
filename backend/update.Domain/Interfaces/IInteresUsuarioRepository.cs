using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IInteresUsuarioRepository : IRepository<InteresUsuario>
{
    Task<IReadOnlyList<InteresUsuario>> ObtenerPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
