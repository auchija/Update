using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de publicaciones_etiquetas.</summary>
public sealed class PublicacionEtiquetaRepository(ApplicationDbContext contexto) : GenericRepository<PublicacionEtiqueta>(contexto), IPublicacionEtiquetaRepository
{
    public async Task<IReadOnlyList<PublicacionEtiqueta>> ObtenerPorPublicacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.PublicacionId == valor, pagina, tamanoPagina, cancellationToken);
}
