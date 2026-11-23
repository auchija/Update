using update.Domain.Entities;

namespace update.Domain.Interfaces;

public interface IServicioUsuario
{
    Task<IReadOnlyList<Usuario>> ListarAsync(int pagina, int tamanoPagina, CancellationToken cancellationToken = default);
    Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default);
    Task<Usuario> CrearAsync(Usuario entrada, CancellationToken cancellationToken = default);
    Task ActualizarAsync(Guid id, Usuario entrada, CancellationToken cancellationToken = default);
    Task<bool> EliminarAsync(Guid id, CancellationToken cancellationToken = default);
}
