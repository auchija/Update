using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IUsuarioTipoPersonaRepository : IRepository<UsuarioTipoPersona>
{
    Task<IReadOnlyList<UsuarioTipoPersona>> ObtenerPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
