using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de variantes_archivo.</summary>
public sealed class VarianteArchivoRepository(ApplicationDbContext contexto) : GenericRepository<VarianteArchivo>(contexto), IVarianteArchivoRepository
{
    public async Task<IReadOnlyList<VarianteArchivo>> ObtenerPorArchivoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.ArchivoId == valor, pagina, tamanoPagina, cancellationToken);
}
