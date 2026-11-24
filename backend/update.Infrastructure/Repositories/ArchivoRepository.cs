using Microsoft.EntityFrameworkCore;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Exceptions;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

public sealed class ArchivoRepository(ApplicationDbContext contexto) : GenericRepository<Archivo>(contexto), IArchivoRepository
{
    public async Task<IReadOnlyList<Archivo>> ObtenerPorSubidoPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.SubidoPorUsuarioId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<Archivo>> ObtenerHistorialAsync(Guid usuarioId, int pagina, int tamanoPagina, CancellationToken ct)
    {
        if (pagina < 1 || pagina > 100000 || tamanoPagina < 1 || tamanoPagina > 100)
            throw new ExcepcionDominio("La página debe estar entre 1 y 100000; el tamaño, entre 1 y 100.");
        return await Conjunto.AsNoTracking()
            .Where(e => e.SubidoPorUsuarioId == usuarioId)
            .OrderByDescending(e => e.CreadoEn).ThenByDescending(e => e.Id)
            .Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina)
            .ToListAsync(ct);
    }

    public async Task<bool> EsDeCuentaPrivadaAsync(Guid usuarioId, CancellationToken ct)
        => await Contexto.Set<Perfil>().AnyAsync(p => p.UsuarioId == usuarioId && p.Tipo == TipoPerfil.Persona && p.EsPrivado, ct);

    public async Task<bool> EnUsoAsync(Guid id, CancellationToken ct)
        => await Contexto.Set<Perfil>().AnyAsync(p => p.FotoArchivoId == id || p.PortadaArchivoId == id, ct)
        || await Contexto.Set<PublicacionArchivo>().AnyAsync(p => p.ArchivoId == id && p.Publicacion.EliminadoEn == null, ct)
        || await Contexto.Set<ProductoArchivo>().AnyAsync(p => p.ArchivoId == id && p.Producto.EliminadoEn == null, ct);

    public async Task<IReadOnlyList<Archivo>> ObtenerPorEstadoAsync(EstadoArchivo valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
