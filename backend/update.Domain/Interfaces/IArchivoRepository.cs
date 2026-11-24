using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IArchivoRepository : IRepository<Archivo>
{
    Task<IReadOnlyList<Archivo>> ObtenerPorSubidoPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<bool> EnUsoAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Archivo>> ObtenerPorEstadoAsync(EstadoArchivo valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
