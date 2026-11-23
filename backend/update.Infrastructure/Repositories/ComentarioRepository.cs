using Microsoft.EntityFrameworkCore;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de comentarios.</summary>
public sealed class ComentarioRepository(ApplicationDbContext contexto) : GenericRepository<Comentario>(contexto), IComentarioRepository
{
    public async Task<IReadOnlyList<Comentario>> ObtenerPorPublicacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.PublicacionId == valor, pagina, tamanoPagina, cancellationToken);

    public async Task<IReadOnlyList<Comentario>> ObtenerConAutorAsync(Guid publicacionId, int pagina, int tamanoPagina, CancellationToken cancellationToken = default)
    {
        if (pagina < 1 || pagina > 100000 || tamanoPagina < 1 || tamanoPagina > 100)
            throw new ExcepcionDominio("La página debe estar entre 1 y 100000; el tamaño, entre 1 y 100.");
        return await Conjunto.AsNoTracking()
            .Include(e => e.PerfilAutor).ThenInclude(p => p.FotoArchivo)
            .Where(e => e.PublicacionId == publicacionId)
            .OrderBy(e => e.CreadoEn).ThenBy(e => e.Id)
            .Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina)
            .ToListAsync(cancellationToken);
    }
}
