using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de usuarios.</summary>
public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<IReadOnlyList<Usuario>> ObtenerPorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Usuario>> ObtenerPorRolAsync(string rol, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Usuario>> ObtenerBloqueadosAsync(CancellationToken cancellationToken = default);
    Task<bool> CorreoExisteAsync(string correo, CancellationToken cancellationToken = default);
}
