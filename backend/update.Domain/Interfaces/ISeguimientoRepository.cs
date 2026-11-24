using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public sealed record PerfilRelacion(Perfil Perfil, string? ClaveFoto, DateTime Fecha);

public interface ISeguimientoRepository : IRepository<Seguimiento>
{
    Task<IReadOnlyList<Seguimiento>> ObtenerPorUsuarioSeguidorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Seguimiento>> ObtenerPorEstadoAsync(EstadoSeguimiento valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyDictionary<Guid, EstadoSeguimiento>> ObtenerEstadosAsync(Guid usuarioId, IReadOnlyCollection<Guid> perfilIds, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PerfilRelacion>> ObtenerSeguidoresAsync(Guid perfilId, EstadoSeguimiento estado, int pagina, int tamanoPagina, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PerfilRelacion>> ObtenerSeguidosAsync(Guid usuarioId, int pagina, int tamanoPagina, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PerfilRelacion>> ObtenerRelacionadosAsync(Guid perfilId, Guid excluirUsuarioId, int cantidad, CancellationToken cancellationToken = default);
}
