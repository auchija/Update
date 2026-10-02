using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de perfiles.</summary>
public interface IPerfilRepository : IRepository<Perfil>
{
    Task<IReadOnlyList<Perfil>> ObtenerPorCiudadIdAsync(int? valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Perfil>> ObtenerPorEstadoAsync(EstadoCuenta valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
