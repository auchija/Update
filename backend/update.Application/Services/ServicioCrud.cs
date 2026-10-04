using update.Domain.Interfaces;
namespace update.Application.Services;

/// <summary>Orquesta operaciones CRUD sin depender de EF Core.</summary>
public sealed class ServicioCrud<T>(IRepository<T> repositorio) where T : class, IEntidadValidable
{
    public async Task<IReadOnlyList<T>> ListarAsync(int pagina, int tamanoPagina, CancellationToken cancellationToken = default)
        => await repositorio.ListarAsync(pagina, tamanoPagina, cancellationToken);
    public async Task<T?> ObtenerAsync(object[] claves, CancellationToken cancellationToken = default)
        => await repositorio.ObtenerPorClaveAsync(claves, cancellationToken);
    public async Task<T> CrearAsync(T entidad, CancellationToken cancellationToken = default)
    {
        await repositorio.AgregarAsync(entidad, cancellationToken);
        return entidad;
    }
    public async Task ActualizarAsync(T entidad, CancellationToken cancellationToken = default)
        => await repositorio.ActualizarAsync(entidad, cancellationToken);
    public async Task<bool> EliminarAsync(object[] claves, CancellationToken cancellationToken = default)
    {
        var entidad = await repositorio.ObtenerPorClaveAsync(claves, cancellationToken);
        if (entidad is null) return false;
        await repositorio.EliminarAsync(entidad, cancellationToken);
        return true;
    }
}
