using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de usuario_tipos_persona.</summary>
public sealed class UsuarioTipoPersonaRepository(ApplicationDbContext contexto) : GenericRepository<UsuarioTipoPersona>(contexto), IUsuarioTipoPersonaRepository
{
    public async Task<IReadOnlyList<UsuarioTipoPersona>> ObtenerPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.UsuarioId == valor, pagina, tamanoPagina, cancellationToken);
}
