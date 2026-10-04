using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de tipos_publicacion.</summary>
public interface ITipoPublicacionRepository : IRepository<TipoPublicacion>
{
    Task<IReadOnlyList<TipoPublicacion>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
