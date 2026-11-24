using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de perfiles.</summary>
public interface IPerfilRepository : IRepository<Perfil>
{
    Task<IReadOnlyList<Perfil>> ObtenerPorCiudadIdAsync(int? valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Perfil>> ObtenerPorEstadoAsync(EstadoCuenta valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    /// <summary>Perfil activo con ese @usuario y su foto; null si no existe.</summary>
    Task<Perfil?> ObtenerActivoPorNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken = default);
    /// <summary>Perfiles de persona activos y con publicaciones, los más activos primero, sin los del usuario indicado.</summary>
    Task<IReadOnlyList<Perfil>> ObtenerSugeridosAsync(Guid excluirUsuarioId, int cantidad, CancellationToken cancellationToken = default);
    /// <summary>Suma (o resta) a los contadores de seguidores y seguidos en una sola sentencia UPDATE.</summary>
    Task AjustarSeguimientoAsync(Guid perfilId, int seguidores, int seguidos, CancellationToken cancellationToken = default);
}
