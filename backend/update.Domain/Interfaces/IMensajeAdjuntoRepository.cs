using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de mensajes_adjuntos.</summary>
public interface IMensajeAdjuntoRepository : IRepository<MensajeAdjunto>
{
    Task<IReadOnlyList<MensajeAdjunto>> ObtenerPorMensajeIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
