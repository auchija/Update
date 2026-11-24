using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IPublicacionArchivoRepository : IRepository<PublicacionArchivo>
{
    Task<IReadOnlyList<PublicacionArchivo>> ObtenerPorArchivoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PublicacionArchivo>> ObtenerPorPublicacionesAsync(IReadOnlyCollection<Guid> publicacionIds, CancellationToken cancellationToken = default);
}
