using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de publicaciones_archivos.</summary>
public interface IPublicacionArchivoRepository : IRepository<PublicacionArchivo>
{
    Task<IReadOnlyList<PublicacionArchivo>> ObtenerPorArchivoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    /// <summary>Archivos de varias publicaciones a la vez, con los datos del archivo, para no consultar uno por uno.</summary>
    Task<IReadOnlyList<PublicacionArchivo>> ObtenerPorPublicacionesAsync(IReadOnlyCollection<Guid> publicacionIds, CancellationToken cancellationToken = default);
}
