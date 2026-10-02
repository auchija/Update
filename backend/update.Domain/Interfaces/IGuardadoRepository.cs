using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de guardados.</summary>
public interface IGuardadoRepository : IRepository<Guardado>
{
    Task<IReadOnlyList<Guardado>> ObtenerPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
