using Microsoft.EntityFrameworkCore;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de publicaciones_archivos.</summary>
public sealed class PublicacionArchivoRepository(ApplicationDbContext contexto) : GenericRepository<PublicacionArchivo>(contexto), IPublicacionArchivoRepository
{
    public async Task<IReadOnlyList<PublicacionArchivo>> ObtenerPorArchivoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.ArchivoId == valor, pagina, tamanoPagina, cancellationToken);

    public async Task<IReadOnlyList<PublicacionArchivo>> ObtenerPorPublicacionesAsync(IReadOnlyCollection<Guid> publicacionIds, CancellationToken cancellationToken = default)
    {
        if (publicacionIds.Count == 0) return [];
        return await Conjunto.AsNoTracking()
            .Include(e => e.Archivo)
            .Where(e => publicacionIds.Contains(e.PublicacionId))
            .OrderBy(e => e.PublicacionId).ThenBy(e => e.Posicion)
            .ToListAsync(cancellationToken);
    }
}
