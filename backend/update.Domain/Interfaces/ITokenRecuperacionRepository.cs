using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de tokens_recuperacion.</summary>
public interface ITokenRecuperacionRepository : IRepository<TokenRecuperacion>
{
    Task<IReadOnlyList<TokenRecuperacion>> ObtenerPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
