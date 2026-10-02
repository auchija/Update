using Microsoft.EntityFrameworkCore;
using update.Domain.Exceptions;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de usuarios.</summary>
public sealed class UsuarioRepository(ApplicationDbContext contexto) : GenericRepository<Usuario>(contexto), IUsuarioRepository
{
    public async Task<IReadOnlyList<Usuario>> ObtenerPorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Id == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken = default)
    {
        var normalizado = correo.Trim().ToLowerInvariant();
        return await Contexto.Usuarios.FirstOrDefaultAsync(u => u.Correo.ToLower() == normalizado, cancellationToken);
    }
    public async Task<IReadOnlyList<Usuario>> ObtenerPorRolAsync(string rol, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<RolPlataforma>(rol, true, out var valor) || !Enum.IsDefined(valor))
            throw new ExcepcionDominio("El rol no es válido.");
        return await Contexto.Usuarios.AsNoTracking().Where(u => u.RolPlataforma == valor).ToListAsync(cancellationToken);
    }
    public async Task<IReadOnlyList<Usuario>> ObtenerBloqueadosAsync(CancellationToken cancellationToken = default)
    {
        var ahora = DateTime.UtcNow;
        return await Contexto.Usuarios.AsNoTracking().Where(u => u.BloqueadoHasta > ahora).ToListAsync(cancellationToken);
    }
    public async Task<bool> CorreoExisteAsync(string correo, CancellationToken cancellationToken = default)
    {
        var normalizado = correo.Trim().ToLowerInvariant();
        return await Contexto.Usuarios.AnyAsync(u => u.Correo.ToLower() == normalizado, cancellationToken);
    }
}
