using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de enlaces_emprendimiento; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/enlaces_emprendimiento")]
public sealed class EnlacesEmprendimientoController(ServicioCrud<EnlaceEmprendimiento> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaEnlaceEmprendimientoDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaEnlaceEmprendimientoDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaEnlaceEmprendimientoDto>> Crear([FromBody] CrearEnlaceEmprendimientoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new EnlaceEmprendimiento
        {
            EmprendimientoId = entrada.EmprendimientoId,
            Plataforma = entrada.Plataforma,
            Url = entrada.Url,
            Orden = entrada.Orden
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarEnlaceEmprendimientoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.EmprendimientoId = entrada.EmprendimientoId;
        entidad.Plataforma = entrada.Plataforma;
        entidad.Url = entrada.Url;
        entidad.Orden = entrada.Orden;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([id], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaEnlaceEmprendimientoDto ARespuesta(EnlaceEmprendimiento entidad) => new()
    {
        Id = entidad.Id,
        EmprendimientoId = entidad.EmprendimientoId,
        Plataforma = entidad.Plataforma,
        Url = entidad.Url,
        Orden = entidad.Orden
    };
}
