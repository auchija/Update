using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface ISesionRepository : IRepository<Sesion>
{
    Task<IReadOnlyList<Sesion>> ObtenerPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
