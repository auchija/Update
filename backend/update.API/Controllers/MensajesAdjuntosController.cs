using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de mensajes_adjuntos; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/mensajes_adjuntos")]
public sealed class MensajesAdjuntosController(ServicioCrud<MensajeAdjunto> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaMensajeAdjuntoDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{mensajeId:guid}/{archivoId:guid}")]
    public async Task<ActionResult<RespuestaMensajeAdjuntoDto>> Obtener(Guid mensajeId, Guid archivoId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([mensajeId, archivoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaMensajeAdjuntoDto>> Crear([FromBody] CrearMensajeAdjuntoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new MensajeAdjunto
        {
            MensajeId = entrada.MensajeId,
            ArchivoId = entrada.ArchivoId,
            Posicion = entrada.Posicion
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { mensajeId = entidad.MensajeId, archivoId = entidad.ArchivoId }, ARespuesta(entidad));
    }

    [HttpPut("{mensajeId:guid}/{archivoId:guid}")]
    public async Task<IActionResult> Actualizar(Guid mensajeId, Guid archivoId, [FromBody] ActualizarMensajeAdjuntoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([mensajeId, archivoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.Posicion = entrada.Posicion;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{mensajeId:guid}/{archivoId:guid}")]
    public async Task<IActionResult> Eliminar(Guid mensajeId, Guid archivoId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([mensajeId, archivoId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaMensajeAdjuntoDto ARespuesta(MensajeAdjunto entidad) => new()
    {
        MensajeId = entidad.MensajeId,
        ArchivoId = entidad.ArchivoId,
        Posicion = entidad.Posicion
    };
}
