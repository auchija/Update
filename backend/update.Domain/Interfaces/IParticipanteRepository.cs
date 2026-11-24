using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IParticipanteRepository : IRepository<Participante>
{
    Task<IReadOnlyList<Participante>> ObtenerPorConversacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
