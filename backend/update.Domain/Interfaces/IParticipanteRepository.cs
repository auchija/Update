using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de participantes.</summary>
public interface IParticipanteRepository : IRepository<Participante>
{
    Task<IReadOnlyList<Participante>> ObtenerPorConversacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
