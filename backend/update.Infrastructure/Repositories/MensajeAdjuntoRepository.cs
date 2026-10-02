using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de mensajes_adjuntos.</summary>
public sealed class MensajeAdjuntoRepository(ApplicationDbContext contexto) : GenericRepository<MensajeAdjunto>(contexto), IMensajeAdjuntoRepository
{
    public async Task<IReadOnlyList<MensajeAdjunto>> ObtenerPorMensajeIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.MensajeId == valor, pagina, tamanoPagina, cancellationToken);
}
