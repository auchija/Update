using Microsoft.EntityFrameworkCore;
using update.Domain.Interfaces;
using update.Domain.Entities;
using update.Infrastructure.Data;

namespace update.Infrastructure.Repositories;

public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObtenerPorCorreoAsync(string correo)
        => await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Correo.ToLower() == correo.ToLower());

    public async Task<IEnumerable<Usuario>> ObtenerPorRolAsync(string rol)
        => await _context.Usuarios
            .Where(u => u.RolPlataforma.ToString() == rol)
            .ToListAsync();

    public async Task<IEnumerable<Usuario>> ObtenerBloqueadosAsync()
        => await _context.Usuarios
            .Where(u => u.BloqueadoHasta.HasValue && u.BloqueadoHasta > DateTime.UtcNow)
            .ToListAsync();

    public async Task<bool> CorreoExisteAsync(string correo)
        => await _context.Usuarios
            .AnyAsync(u => u.Correo.ToLower() == correo.ToLower());
}
