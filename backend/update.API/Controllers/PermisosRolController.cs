using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de permisos_rol; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/permisos_rol")]
public sealed class PermisosRolController(ServicioCrud<PermisoRol> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaPermisoRolDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{rol}/{permiso}")]
    public async Task<ActionResult<RespuestaPermisoRolDto>> Obtener(RolEmprendimiento rol, string permiso, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([rol, permiso], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaPermisoRolDto>> Crear([FromBody] CrearPermisoRolDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new PermisoRol
        {
            Rol = entrada.Rol,
            Permiso = entrada.Permiso
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { rol = entidad.Rol, permiso = entidad.Permiso }, ARespuesta(entidad));
    }

    [HttpPut("{rol}/{permiso}")]
    public async Task<IActionResult> Actualizar(RolEmprendimiento rol, string permiso, [FromBody] ActualizarPermisoRolDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([rol, permiso], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });

        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{rol}/{permiso}")]
    public async Task<IActionResult> Eliminar(RolEmprendimiento rol, string permiso, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([rol, permiso], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaPermisoRolDto ARespuesta(PermisoRol entidad) => new()
    {
        Rol = entidad.Rol,
        Permiso = entidad.Permiso
    };
}
