using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de etiquetas.</summary>
public sealed class EtiquetaRepository(ApplicationDbContext contexto) : GenericRepository<Etiqueta>(contexto), IEtiquetaRepository
{
    public async Task<IReadOnlyList<Etiqueta>> ObtenerPorIdAsync(int valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Id == valor, pagina, tamanoPagina, cancellationToken);
}
