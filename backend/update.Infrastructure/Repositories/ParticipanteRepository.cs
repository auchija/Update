using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de participantes.</summary>
public sealed class ParticipanteRepository(ApplicationDbContext contexto) : GenericRepository<Participante>(contexto), IParticipanteRepository
{
    public async Task<IReadOnlyList<Participante>> ObtenerPorConversacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.ConversacionId == valor, pagina, tamanoPagina, cancellationToken);
}
