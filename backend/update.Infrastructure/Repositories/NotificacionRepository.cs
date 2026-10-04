using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de notificaciones.</summary>
public sealed class NotificacionRepository(ApplicationDbContext contexto) : GenericRepository<Notificacion>(contexto), INotificacionRepository
{
    public async Task<IReadOnlyList<Notificacion>> ObtenerPorUsuarioDestinatarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.UsuarioDestinatarioId == valor, pagina, tamanoPagina, cancellationToken);
}
