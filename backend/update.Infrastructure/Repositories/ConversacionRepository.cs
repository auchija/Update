using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de conversaciones.</summary>
public sealed class ConversacionRepository(ApplicationDbContext contexto) : GenericRepository<Conversacion>(contexto), IConversacionRepository
{
    public async Task<IReadOnlyList<Conversacion>> ObtenerPorCreadoPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.CreadoPorUsuarioId == valor, pagina, tamanoPagina, cancellationToken);
}
