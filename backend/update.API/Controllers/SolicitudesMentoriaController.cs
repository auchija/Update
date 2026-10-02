using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de solicitudes_mentoria; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/solicitudes_mentoria")]
public sealed class SolicitudesMentoriaController(ServicioCrud<SolicitudMentoria> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaSolicitudMentoriaDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaSolicitudMentoriaDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaSolicitudMentoriaDto>> Crear([FromBody] CrearSolicitudMentoriaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new SolicitudMentoria
        {
            OfertaId = entrada.OfertaId,
            PerfilMentoreadoId = entrada.PerfilMentoreadoId,
            CreadoPorUsuarioId = entrada.CreadoPorUsuarioId,
            Mensaje = entrada.Mensaje,
            Estado = entrada.Estado,
            AgendadoEn = entrada.AgendadoEn,
            UrlReunion = entrada.UrlReunion,
            CalificacionMentoreado = entrada.CalificacionMentoreado,
            ComentarioMentoreado = entrada.ComentarioMentoreado
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarSolicitudMentoriaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.OfertaId = entrada.OfertaId;
        entidad.PerfilMentoreadoId = entrada.PerfilMentoreadoId;
        entidad.CreadoPorUsuarioId = entrada.CreadoPorUsuarioId;
        entidad.Mensaje = entrada.Mensaje;
        entidad.Estado = entrada.Estado;
        entidad.AgendadoEn = entrada.AgendadoEn;
        entidad.UrlReunion = entrada.UrlReunion;
        entidad.CalificacionMentoreado = entrada.CalificacionMentoreado;
        entidad.ComentarioMentoreado = entrada.ComentarioMentoreado;
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

    private static RespuestaSolicitudMentoriaDto ARespuesta(SolicitudMentoria entidad) => new()
    {
        Id = entidad.Id,
        OfertaId = entidad.OfertaId,
        PerfilMentoreadoId = entidad.PerfilMentoreadoId,
        CreadoPorUsuarioId = entidad.CreadoPorUsuarioId,
        Mensaje = entidad.Mensaje,
        Estado = entidad.Estado,
        AgendadoEn = entidad.AgendadoEn,
        UrlReunion = entidad.UrlReunion,
        CalificacionMentoreado = entidad.CalificacionMentoreado,
        ComentarioMentoreado = entidad.ComentarioMentoreado,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn
    };
}
