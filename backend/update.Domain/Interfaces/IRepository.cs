using System.Linq.Expressions;
namespace update.Domain.Interfaces;

public interface IRepository<T> where T : class, IEntidadValidable
{
    Task<IReadOnlyList<T>> ListarAsync(int pagina, int tamanoPagina, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> BuscarAsync(Expression<Func<T, bool>> criterio, int pagina, int tamanoPagina, CancellationToken cancellationToken = default);
    Task<T?> ObtenerPorClaveAsync(object[] claves, CancellationToken cancellationToken = default);
    Task AgregarAsync(T entidad, CancellationToken cancellationToken = default);
    Task ActualizarAsync(T entidad, CancellationToken cancellationToken = default);
    Task EliminarAsync(T entidad, CancellationToken cancellationToken = default);
}
