using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface ITipoPublicacionRepository : IRepository<TipoPublicacion>
{
    Task<IReadOnlyList<TipoPublicacion>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task AsegurarAsync(string codigo, string nombre, CancellationToken cancellationToken = default);
}
