using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de oportunidades.</summary>
public interface IOportunidadRepository : IRepository<Oportunidad>
{
    Task<IReadOnlyList<Oportunidad>> ObtenerPorPerfilAutorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Oportunidad>> ObtenerPorEstadoAsync(EstadoOportunidad valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
