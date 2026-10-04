using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de publicaciones_etiquetas.</summary>
public interface IPublicacionEtiquetaRepository : IRepository<PublicacionEtiqueta>
{
    Task<IReadOnlyList<PublicacionEtiqueta>> ObtenerPorPublicacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
