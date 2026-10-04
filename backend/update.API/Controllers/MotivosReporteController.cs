using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de motivos_reporte; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/motivos_reporte")]
public sealed class MotivosReporteController(ServicioCrud<MotivoReporte> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaMotivoReporteDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{codigo}")]
    public async Task<ActionResult<RespuestaMotivoReporteDto>> Obtener(string codigo, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([codigo], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaMotivoReporteDto>> Crear([FromBody] CrearMotivoReporteDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new MotivoReporte
        {
            Codigo = entrada.Codigo,
            Nombre = entrada.Nombre,
            Orden = entrada.Orden,
            Activo = entrada.Activo
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { codigo = entidad.Codigo }, ARespuesta(entidad));
    }

    [HttpPut("{codigo}")]
    public async Task<IActionResult> Actualizar(string codigo, [FromBody] ActualizarMotivoReporteDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([codigo], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.Nombre = entrada.Nombre;
        entidad.Orden = entrada.Orden;
        entidad.Activo = entrada.Activo;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{codigo}")]
    public async Task<IActionResult> Eliminar(string codigo, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([codigo], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaMotivoReporteDto ARespuesta(MotivoReporte entidad) => new()
    {
        Codigo = entidad.Codigo,
        Nombre = entidad.Nombre,
        Orden = entidad.Orden,
        Activo = entidad.Activo
    };
}
