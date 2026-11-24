using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Un perfil en una lista de seguimiento, con la clave de su foto y la fecha de la relación.</summary>
public sealed record PerfilRelacion(Perfil Perfil, string? ClaveFoto, DateTime Fecha);

/// <summary>Consultas específicas de seguimientos.</summary>
public interface ISeguimientoRepository : IRepository<Seguimiento>
{
    Task<IReadOnlyList<Seguimiento>> ObtenerPorUsuarioSeguidorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Seguimiento>> ObtenerPorEstadoAsync(EstadoSeguimiento valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    /// <summary>Para esos perfiles, el estado del seguimiento del usuario (los que no sigue no aparecen).</summary>
    Task<IReadOnlyDictionary<Guid, EstadoSeguimiento>> ObtenerEstadosAsync(Guid usuarioId, IReadOnlyCollection<Guid> perfilIds, CancellationToken cancellationToken = default);
    /// <summary>Perfiles (de persona) de quienes siguen al perfil con ese estado; los más recientes primero.</summary>
    Task<IReadOnlyList<PerfilRelacion>> ObtenerSeguidoresAsync(Guid perfilId, EstadoSeguimiento estado, int pagina, int tamanoPagina, CancellationToken cancellationToken = default);
    /// <summary>Perfiles que el usuario sigue (activos); los más recientes primero.</summary>
    Task<IReadOnlyList<PerfilRelacion>> ObtenerSeguidosAsync(Guid usuarioId, int pagina, int tamanoPagina, CancellationToken cancellationToken = default);
    /// <summary>
    /// Perfiles relacionados: los que más siguen también los seguidores del perfil.
    /// Sin el propio perfil ni los del usuario indicado.
    /// </summary>
    Task<IReadOnlyList<PerfilRelacion>> ObtenerRelacionadosAsync(Guid perfilId, Guid excluirUsuarioId, int cantidad, CancellationToken cancellationToken = default);
}
