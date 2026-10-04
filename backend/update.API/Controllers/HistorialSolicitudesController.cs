using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de historial_solicitudes; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/historial_solicitudes")]
public sealed class HistorialSolicitudesController(ServicioCrud<HistorialSolicitud> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaHistorialSolicitudDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<RespuestaHistorialSolicitudDto>> Obtener(long id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaHistorialSolicitudDto>> Crear([FromBody] CrearHistorialSolicitudDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new HistorialSolicitud
        {
            SolicitudCotizacionId = entrada.SolicitudCotizacionId,
            EstadoAnterior = entrada.EstadoAnterior,
            EstadoNuevo = entrada.EstadoNuevo,
            UsuarioActorId = entrada.UsuarioActorId,
            Nota = entrada.Nota
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Actualizar(long id, [FromBody] ActualizarHistorialSolicitudDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.SolicitudCotizacionId = entrada.SolicitudCotizacionId;
        entidad.EstadoAnterior = entrada.EstadoAnterior;
        entidad.EstadoNuevo = entrada.EstadoNuevo;
        entidad.UsuarioActorId = entrada.UsuarioActorId;
        entidad.Nota = entrada.Nota;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Eliminar(long id, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([id], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaHistorialSolicitudDto ARespuesta(HistorialSolicitud entidad) => new()
    {
        Id = entidad.Id,
        SolicitudCotizacionId = entidad.SolicitudCotizacionId,
        EstadoAnterior = entidad.EstadoAnterior,
        EstadoNuevo = entidad.EstadoNuevo,
        UsuarioActorId = entidad.UsuarioActorId,
        Nota = entidad.Nota,
        CreadoEn = entidad.CreadoEn
    };
}
