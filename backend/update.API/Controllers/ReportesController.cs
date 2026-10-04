using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de reportes; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/reportes")]
public sealed class ReportesController(ServicioCrud<Reporte> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaReporteDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaReporteDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaReporteDto>> Crear([FromBody] CrearReporteDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Reporte
        {
            UsuarioReportanteId = entrada.UsuarioReportanteId,
            ObjetivoTipo = entrada.ObjetivoTipo,
            ObjetivoId = entrada.ObjetivoId,
            MotivoCodigo = entrada.MotivoCodigo,
            Detalles = entrada.Detalles,
            Estado = entrada.Estado,
            ResueltoPorUsuarioId = entrada.ResueltoPorUsuarioId,
            NotaResolucion = entrada.NotaResolucion,
            ResueltoEn = entrada.ResueltoEn
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarReporteDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.UsuarioReportanteId = entrada.UsuarioReportanteId;
        entidad.ObjetivoTipo = entrada.ObjetivoTipo;
        entidad.ObjetivoId = entrada.ObjetivoId;
        entidad.MotivoCodigo = entrada.MotivoCodigo;
        entidad.Detalles = entrada.Detalles;
        entidad.Estado = entrada.Estado;
        entidad.ResueltoPorUsuarioId = entrada.ResueltoPorUsuarioId;
        entidad.NotaResolucion = entrada.NotaResolucion;
        entidad.ResueltoEn = entrada.ResueltoEn;
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

    private static RespuestaReporteDto ARespuesta(Reporte entidad) => new()
    {
        Id = entidad.Id,
        UsuarioReportanteId = entidad.UsuarioReportanteId,
        ObjetivoTipo = entidad.ObjetivoTipo,
        ObjetivoId = entidad.ObjetivoId,
        MotivoCodigo = entidad.MotivoCodigo,
        Detalles = entidad.Detalles,
        Estado = entidad.Estado,
        ResueltoPorUsuarioId = entidad.ResueltoPorUsuarioId,
        NotaResolucion = entidad.NotaResolucion,
        CreadoEn = entidad.CreadoEn,
        ResueltoEn = entidad.ResueltoEn
    };
}
