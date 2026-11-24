using Microsoft.EntityFrameworkCore;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

public sealed class ArchivoRepository(ApplicationDbContext contexto) : GenericRepository<Archivo>(contexto), IArchivoRepository
{
    public async Task<IReadOnlyList<Archivo>> ObtenerPorSubidoPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.SubidoPorUsuarioId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<bool> EnUsoAsync(Guid id, CancellationToken ct)
        => await Contexto.Set<Perfil>().AnyAsync(p => p.FotoArchivoId == id || p.PortadaArchivoId == id, ct)
        || await Contexto.Set<PublicacionArchivo>().AnyAsync(p => p.ArchivoId == id && p.Publicacion.EliminadoEn == null, ct)
        || await Contexto.Set<ProductoArchivo>().AnyAsync(p => p.ArchivoId == id && p.Producto.EliminadoEn == null, ct);

    public async Task<IReadOnlyList<Archivo>> ObtenerPorEstadoAsync(EstadoArchivo valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
