using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de verificaciones; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/verificaciones")]
public sealed class VerificacionesController(ServicioCrud<Verificacion> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaVerificacionDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaVerificacionDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaVerificacionDto>> Crear([FromBody] CrearVerificacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Verificacion
        {
            EmprendimientoId = entrada.EmprendimientoId,
            Estado = entrada.Estado,
            Metodo = entrada.Metodo,
            EnviadoPorUsuarioId = entrada.EnviadoPorUsuarioId,
            RevisadoPorUsuarioId = entrada.RevisadoPorUsuarioId,
            NotasRevisor = entrada.NotasRevisor,
            RevisadoEn = entrada.RevisadoEn
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarVerificacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.EmprendimientoId = entrada.EmprendimientoId;
        entidad.Estado = entrada.Estado;
        entidad.Metodo = entrada.Metodo;
        entidad.EnviadoPorUsuarioId = entrada.EnviadoPorUsuarioId;
        entidad.RevisadoPorUsuarioId = entrada.RevisadoPorUsuarioId;
        entidad.NotasRevisor = entrada.NotasRevisor;
        entidad.RevisadoEn = entrada.RevisadoEn;
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

    private static RespuestaVerificacionDto ARespuesta(Verificacion entidad) => new()
    {
        Id = entidad.Id,
        EmprendimientoId = entidad.EmprendimientoId,
        Estado = entidad.Estado,
        Metodo = entidad.Metodo,
        EnviadoPorUsuarioId = entidad.EnviadoPorUsuarioId,
        RevisadoPorUsuarioId = entidad.RevisadoPorUsuarioId,
        NotasRevisor = entidad.NotasRevisor,
        EnviadoEn = entidad.EnviadoEn,
        RevisadoEn = entidad.RevisadoEn
    };
}
