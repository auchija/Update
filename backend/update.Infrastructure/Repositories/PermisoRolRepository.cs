using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de permisos_rol.</summary>
public sealed class PermisoRolRepository(ApplicationDbContext contexto) : GenericRepository<PermisoRol>(contexto), IPermisoRolRepository
{
    public async Task<IReadOnlyList<PermisoRol>> ObtenerPorRolAsync(RolEmprendimiento valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Rol == valor, pagina, tamanoPagina, cancellationToken);
}
