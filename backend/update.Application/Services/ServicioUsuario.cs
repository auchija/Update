using update.Domain.Entities;
using update.Domain.Interfaces;

namespace update.Application.Services;

public sealed class ServicioUsuario : IServicioUsuario
{
    private readonly IUsuarioRepository _repositorio;
    private readonly ServicioCrud<Usuario> _crud;

    public ServicioUsuario(IUsuarioRepository repositorio)
    {
        _repositorio = repositorio;
        _crud = new ServicioCrud<Usuario>(repositorio);
    }

    public Task<IReadOnlyList<Usuario>> ListarAsync(
        int pagina,
        int tamanoPagina,
        CancellationToken cancellationToken = default)
    {
        ValidarPaginacion(pagina, tamanoPagina);
        return _crud.ListarAsync(pagina, tamanoPagina, cancellationToken);
    }

    public Task<Usuario?> ObtenerPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidarId(id);
        return _crud.ObtenerAsync([id], cancellationToken);
    }

    public Task<Usuario?> ObtenerPorCorreoAsync(
        string correo,
        CancellationToken cancellationToken = default)
    {
        ValidarCorreo(correo);
        return _repositorio.ObtenerPorCorreoAsync(correo.Trim(), cancellationToken);
    }

    public async Task<Usuario> CrearAsync(
        Usuario entrada,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entrada);
        ValidarCorreo(entrada.Correo);

        entrada.Correo = entrada.Correo.Trim();

        if (await _repositorio.CorreoExisteAsync(entrada.Correo, cancellationToken))
            throw new InvalidOperationException("Ya existe un usuario con ese correo.");

        return await _crud.CrearAsync(entrada, cancellationToken);
    }

    public async Task ActualizarAsync(
        Guid id,
        Usuario entrada,
        CancellationToken cancellationToken = default)
    {
        ValidarId(id);
        ArgumentNullException.ThrowIfNull(entrada);
        ValidarCorreo(entrada.Correo);

        var existente = await ObtenerPorIdAsync(id, cancellationToken);
        if (existente is null)
            throw new KeyNotFoundException($"No se encontró el usuario {id}.");

        entrada.Correo = entrada.Correo.Trim();

        var usuarioConCorreo = await _repositorio.ObtenerPorCorreoAsync(
            entrada.Correo,
            cancellationToken);

        if (usuarioConCorreo is not null && usuarioConCorreo.Id != id)
            throw new InvalidOperationException("Ya existe otro usuario con ese correo.");

        // El ID de la ruta es la fuente de verdad.
        entrada.Id = id;

        await _crud.ActualizarAsync(entrada, cancellationToken);
    }

    public async Task<bool> EliminarAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        ValidarId(id);
        return await _crud.EliminarAsync([id], cancellationToken);
    }

    private static void ValidarId(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("El id no puede estar vacío.", nameof(id));
    }

    private static void ValidarCorreo(string? correo)
    {
        if (string.IsNullOrWhiteSpace(correo))
            throw new ArgumentException("El correo es obligatorio.", nameof(correo));
    }

    private static void ValidarPaginacion(int pagina, int tamanoPagina)
    {
        if (pagina < 1)
            throw new ArgumentOutOfRangeException(nameof(pagina), "La página debe ser mayor que cero.");

        if (tamanoPagina < 1)
            throw new ArgumentOutOfRangeException(nameof(tamanoPagina), "El tamaño de página debe ser mayor que cero.");
    }
}
