using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IPerfilRepository : IRepository<Perfil>
{
    Task<IReadOnlyList<Perfil>> ObtenerPorCiudadIdAsync(int? valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Perfil>> ObtenerPorEstadoAsync(EstadoCuenta valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<Perfil?> ObtenerActivoPorNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Perfil>> ObtenerSugeridosAsync(Guid excluirUsuarioId, int cantidad, CancellationToken cancellationToken = default);
    Task AjustarSeguimientoAsync(Guid perfilId, int seguidores, int seguidos, CancellationToken cancellationToken = default);
}
