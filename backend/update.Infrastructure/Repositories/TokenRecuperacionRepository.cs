using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de tokens_recuperacion.</summary>
public sealed class TokenRecuperacionRepository(ApplicationDbContext contexto) : GenericRepository<TokenRecuperacion>(contexto), ITokenRecuperacionRepository
{
    public async Task<IReadOnlyList<TokenRecuperacion>> ObtenerPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.UsuarioId == valor, pagina, tamanoPagina, cancellationToken);
}
