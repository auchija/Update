using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IMensajeAdjuntoRepository : IRepository<MensajeAdjunto>
{
    Task<IReadOnlyList<MensajeAdjunto>> ObtenerPorMensajeIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
