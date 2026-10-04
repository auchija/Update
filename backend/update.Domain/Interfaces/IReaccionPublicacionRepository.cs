using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de reacciones_publicacion.</summary>
public interface IReaccionPublicacionRepository : IRepository<ReaccionPublicacion>
{
    Task<IReadOnlyList<ReaccionPublicacion>> ObtenerPorPublicacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
