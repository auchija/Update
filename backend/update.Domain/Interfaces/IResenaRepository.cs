using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de resenas.</summary>
public interface IResenaRepository : IRepository<Resena>
{
    Task<IReadOnlyList<Resena>> ObtenerPorUsuarioResenadorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Resena>> ObtenerPorEstadoAsync(EstadoResena valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
