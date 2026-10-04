using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de publicaciones.</summary>
public sealed class PublicacionRepository(ApplicationDbContext contexto) : GenericRepository<Publicacion>(contexto), IPublicacionRepository
{
    public async Task<IReadOnlyList<Publicacion>> ObtenerPorPerfilAutorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.PerfilAutorId == valor, pagina, tamanoPagina, cancellationToken);
}
