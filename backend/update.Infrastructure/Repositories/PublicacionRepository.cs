using Microsoft.EntityFrameworkCore;
using update.Domain.Exceptions;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de publicaciones.</summary>
public sealed class PublicacionRepository(ApplicationDbContext contexto) : GenericRepository<Publicacion>(contexto), IPublicacionRepository
{
    public async Task<IReadOnlyList<Publicacion>> ObtenerFeedAsync(Guid usuarioId, int pagina, int tamanoPagina, CancellationToken cancellationToken = default)
    {
        if (pagina < 1 || pagina > 100000 || tamanoPagina < 1 || tamanoPagina > 100)
            throw new ExcepcionDominio("Paginacion no valida.");

        return await Conjunto.AsNoTracking().Include(p => p.PerfilAutor)
            .Where(p => p.EliminadoEn == null && p.PerfilAutor.EliminadoEn == null
                && p.PerfilAutor.Estado == EstadoCuenta.Activo
                && ((p.Visibilidad == VisibilidadPublicacion.Publico && !p.PerfilAutor.EsPrivado)
                    || p.CreadoPorUsuarioId == usuarioId))
            .OrderByDescending(p => p.CreadoEn).ThenByDescending(p => p.Id)
            .Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina)
            .ToListAsync(cancellationToken);
    }

    public async Task<Publicacion?> ObtenerConAutorAsync(Guid id, CancellationToken cancellationToken = default)
        => await Conjunto.Include(p => p.PerfilAutor)
            .FirstOrDefaultAsync(p => p.Id == id && p.EliminadoEn == null, cancellationToken);

    public async Task<IReadOnlyList<Publicacion>> ObtenerPorPerfilAutorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.PerfilAutorId == valor, pagina, tamanoPagina, cancellationToken);
}
