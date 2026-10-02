using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de conversaciones.</summary>
public interface IConversacionRepository : IRepository<Conversacion>
{
    Task<IReadOnlyList<Conversacion>> ObtenerPorCreadoPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
