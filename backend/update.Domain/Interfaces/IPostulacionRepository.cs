using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de postulaciones.</summary>
public interface IPostulacionRepository : IRepository<Postulacion>
{
    Task<IReadOnlyList<Postulacion>> ObtenerPorOportunidadIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Postulacion>> ObtenerPorEstadoAsync(EstadoPostulacion valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
