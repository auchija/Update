using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de postulaciones; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/postulaciones")]
public sealed class PostulacionesController(ServicioCrud<Postulacion> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaPostulacionDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaPostulacionDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaPostulacionDto>> Crear([FromBody] CrearPostulacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Postulacion
        {
            OportunidadId = entrada.OportunidadId,
            PerfilPostulanteId = entrada.PerfilPostulanteId,
            CreadoPorUsuarioId = entrada.CreadoPorUsuarioId,
            Mensaje = entrada.Mensaje,
            Estado = entrada.Estado
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarPostulacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.OportunidadId = entrada.OportunidadId;
        entidad.PerfilPostulanteId = entrada.PerfilPostulanteId;
        entidad.CreadoPorUsuarioId = entrada.CreadoPorUsuarioId;
        entidad.Mensaje = entrada.Mensaje;
        entidad.Estado = entrada.Estado;
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

    private static RespuestaPostulacionDto ARespuesta(Postulacion entidad) => new()
    {
        Id = entidad.Id,
        OportunidadId = entidad.OportunidadId,
        PerfilPostulanteId = entidad.PerfilPostulanteId,
        CreadoPorUsuarioId = entidad.CreadoPorUsuarioId,
        Mensaje = entidad.Mensaje,
        Estado = entidad.Estado,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn
    };
}
