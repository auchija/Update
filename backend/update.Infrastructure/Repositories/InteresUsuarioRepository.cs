using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de intereses_usuario.</summary>
public sealed class InteresUsuarioRepository(ApplicationDbContext contexto) : GenericRepository<InteresUsuario>(contexto), IInteresUsuarioRepository
{
    public async Task<IReadOnlyList<InteresUsuario>> ObtenerPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.UsuarioId == valor, pagina, tamanoPagina, cancellationToken);
}
