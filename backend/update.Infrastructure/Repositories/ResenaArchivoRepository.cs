using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de resenas_archivos.</summary>
public sealed class ResenaArchivoRepository(ApplicationDbContext contexto) : GenericRepository<ResenaArchivo>(contexto), IResenaArchivoRepository
{
    public async Task<IReadOnlyList<ResenaArchivo>> ObtenerPorResenaIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.ResenaId == valor, pagina, tamanoPagina, cancellationToken);
}
