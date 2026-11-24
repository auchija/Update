using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface ISolicitudMentoriaRepository : IRepository<SolicitudMentoria>
{
    Task<IReadOnlyList<SolicitudMentoria>> ObtenerPorOfertaIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SolicitudMentoria>> ObtenerPorEstadoAsync(EstadoMentoria valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
