using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de archivos.</summary>
public sealed class ArchivoRepository(ApplicationDbContext contexto) : GenericRepository<Archivo>(contexto), IArchivoRepository
{
    public async Task<IReadOnlyList<Archivo>> ObtenerPorSubidoPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.SubidoPorUsuarioId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<Archivo>> ObtenerPorEstadoAsync(EstadoArchivo valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
