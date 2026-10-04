using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de miembros_emprendimiento; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/miembros_emprendimiento")]
public sealed class MiembrosEmprendimientoController(ServicioCrud<MiembroEmprendimiento> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaMiembroEmprendimientoDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{emprendimientoId:guid}/{usuarioId:guid}")]
    public async Task<ActionResult<RespuestaMiembroEmprendimientoDto>> Obtener(Guid emprendimientoId, Guid usuarioId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([emprendimientoId, usuarioId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaMiembroEmprendimientoDto>> Crear([FromBody] CrearMiembroEmprendimientoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new MiembroEmprendimiento
        {
            EmprendimientoId = entrada.EmprendimientoId,
            UsuarioId = entrada.UsuarioId,
            Rol = entrada.Rol,
            Titulo = entrada.Titulo,
            Estado = entrada.Estado,
            InvitadoPorUsuarioId = entrada.InvitadoPorUsuarioId,
            UnidoEn = entrada.UnidoEn
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { emprendimientoId = entidad.EmprendimientoId, usuarioId = entidad.UsuarioId }, ARespuesta(entidad));
    }

    [HttpPut("{emprendimientoId:guid}/{usuarioId:guid}")]
    public async Task<IActionResult> Actualizar(Guid emprendimientoId, Guid usuarioId, [FromBody] ActualizarMiembroEmprendimientoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([emprendimientoId, usuarioId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.Rol = entrada.Rol;
        entidad.Titulo = entrada.Titulo;
        entidad.Estado = entrada.Estado;
        entidad.InvitadoPorUsuarioId = entrada.InvitadoPorUsuarioId;
        entidad.UnidoEn = entrada.UnidoEn;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{emprendimientoId:guid}/{usuarioId:guid}")]
    public async Task<IActionResult> Eliminar(Guid emprendimientoId, Guid usuarioId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([emprendimientoId, usuarioId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaMiembroEmprendimientoDto ARespuesta(MiembroEmprendimiento entidad) => new()
    {
        EmprendimientoId = entidad.EmprendimientoId,
        UsuarioId = entidad.UsuarioId,
        Rol = entidad.Rol,
        Titulo = entidad.Titulo,
        Estado = entidad.Estado,
        InvitadoPorUsuarioId = entidad.InvitadoPorUsuarioId,
        CreadoEn = entidad.CreadoEn,
        UnidoEn = entidad.UnidoEn,
        ActualizadoEn = entidad.ActualizadoEn
    };
}
