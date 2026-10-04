using update.Domain.Entities;

namespace update.Domain.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> ObtenerPorCorreoAsync(string correo);
    Task<IEnumerable<Usuario>> ObtenerPorRolAsync(string rol);
    Task<IEnumerable<Usuario>> ObtenerBloqueadosAsync();
    Task<bool> CorreoExisteAsync(string correo);
}
