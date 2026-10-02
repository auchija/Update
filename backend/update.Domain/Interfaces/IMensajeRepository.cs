using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de mensajes.</summary>
public interface IMensajeRepository : IRepository<Mensaje>
{
    Task<IReadOnlyList<Mensaje>> ObtenerPorUsuarioRemitenteIdAsync(Guid? valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
