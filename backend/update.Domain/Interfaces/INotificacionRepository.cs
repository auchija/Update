using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de notificaciones.</summary>
public interface INotificacionRepository : IRepository<Notificacion>
{
    Task<IReadOnlyList<Notificacion>> ObtenerPorUsuarioDestinatarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
