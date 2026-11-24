using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IReaccionPublicacionRepository : IRepository<ReaccionPublicacion>
{
    Task<IReadOnlyList<ReaccionPublicacion>> ObtenerPorPublicacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Guid>> ObtenerPublicacionesConReaccionAsync(Guid perfilId, IReadOnlyCollection<Guid> publicacionIds, CancellationToken cancellationToken = default);
    Task AsegurarTipoAsync(string codigo, string nombre, string emoji, short orden, CancellationToken cancellationToken = default);
    Task<bool> AgregarSiNoExisteAsync(Guid publicacionId, Guid perfilId, string tipo, CancellationToken cancellationToken = default);
    Task<bool> QuitarAsync(Guid publicacionId, Guid perfilId, CancellationToken cancellationToken = default);
}
