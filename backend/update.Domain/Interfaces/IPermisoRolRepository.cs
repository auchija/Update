using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de permisos_rol.</summary>
public interface IPermisoRolRepository : IRepository<PermisoRol>
{
    Task<IReadOnlyList<PermisoRol>> ObtenerPorRolAsync(RolEmprendimiento valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
