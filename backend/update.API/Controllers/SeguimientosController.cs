using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de seguimientos; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/seguimientos")]
public sealed class SeguimientosController(ServicioCrud<Seguimiento> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaSeguimientoDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{usuarioSeguidorId:guid}/{perfilSeguidoId:guid}")]
    public async Task<ActionResult<RespuestaSeguimientoDto>> Obtener(Guid usuarioSeguidorId, Guid perfilSeguidoId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([usuarioSeguidorId, perfilSeguidoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaSeguimientoDto>> Crear([FromBody] CrearSeguimientoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Seguimiento
        {
            UsuarioSeguidorId = entrada.UsuarioSeguidorId,
            PerfilSeguidoId = entrada.PerfilSeguidoId,
            Estado = entrada.Estado,
            AceptadoEn = entrada.AceptadoEn
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { usuarioSeguidorId = entidad.UsuarioSeguidorId, perfilSeguidoId = entidad.PerfilSeguidoId }, ARespuesta(entidad));
    }

    [HttpPut("{usuarioSeguidorId:guid}/{perfilSeguidoId:guid}")]
    public async Task<IActionResult> Actualizar(Guid usuarioSeguidorId, Guid perfilSeguidoId, [FromBody] ActualizarSeguimientoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([usuarioSeguidorId, perfilSeguidoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.Estado = entrada.Estado;
        entidad.AceptadoEn = entrada.AceptadoEn;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{usuarioSeguidorId:guid}/{perfilSeguidoId:guid}")]
    public async Task<IActionResult> Eliminar(Guid usuarioSeguidorId, Guid perfilSeguidoId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([usuarioSeguidorId, perfilSeguidoId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaSeguimientoDto ARespuesta(Seguimiento entidad) => new()
    {
        UsuarioSeguidorId = entidad.UsuarioSeguidorId,
        PerfilSeguidoId = entidad.PerfilSeguidoId,
        Estado = entidad.Estado,
        CreadoEn = entidad.CreadoEn,
        AceptadoEn = entidad.AceptadoEn
    };
}
