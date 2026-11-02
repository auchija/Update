using Microsoft.EntityFrameworkCore;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de publicaciones.</summary>
public sealed class PublicacionRepository(ApplicationDbContext contexto) : GenericRepository<Publicacion>(contexto), IPublicacionRepository
{
    public async Task<IReadOnlyList<Publicacion>> ObtenerPorPerfilAutorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.PerfilAutorId == valor, pagina, tamanoPagina, cancellationToken);

    public async Task<IReadOnlyList<Publicacion>> ObtenerRecientesAsync(int pagina, int tamanoPagina, CancellationToken cancellationToken = default)
    {
        if (pagina < 1 || pagina > 100000 || tamanoPagina < 1 || tamanoPagina > 100)
            throw new ExcepcionDominio("La página debe estar entre 1 y 100000; el tamaño, entre 1 y 100.");
        // Una sola consulta trae el autor y su foto; el Id desempata publicaciones del mismo instante.
        return await Conjunto.AsNoTracking()
            .Include(e => e.PerfilAutor).ThenInclude(p => p.FotoArchivo)
            .Where(e => e.Visibilidad == VisibilidadPublicacion.Publico)
            .OrderByDescending(e => e.CreadoEn).ThenByDescending(e => e.Id)
            .Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina)
            .ToListAsync(cancellationToken);
    }
}
