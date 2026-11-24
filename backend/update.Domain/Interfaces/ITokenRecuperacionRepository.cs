using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface ITokenRecuperacionRepository : IRepository<TokenRecuperacion>
{
    Task<IReadOnlyList<TokenRecuperacion>> ObtenerPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
