using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de conversaciones; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/conversaciones")]
public sealed class ConversacionesController(ServicioCrud<Conversacion> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaConversacionDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaConversacionDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaConversacionDto>> Crear([FromBody] CrearConversacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Conversacion
        {
            Tipo = entrada.Tipo,
            Titulo = entrada.Titulo,
            ClaveDirecta = entrada.ClaveDirecta,
            CreadoPorUsuarioId = entrada.CreadoPorUsuarioId,
            UltimoMensajeId = entrada.UltimoMensajeId,
            UltimoMensajeEn = entrada.UltimoMensajeEn
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarConversacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.Tipo = entrada.Tipo;
        entidad.Titulo = entrada.Titulo;
        entidad.ClaveDirecta = entrada.ClaveDirecta;
        entidad.CreadoPorUsuarioId = entrada.CreadoPorUsuarioId;
        entidad.UltimoMensajeId = entrada.UltimoMensajeId;
        entidad.UltimoMensajeEn = entrada.UltimoMensajeEn;
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

    private static RespuestaConversacionDto ARespuesta(Conversacion entidad) => new()
    {
        Id = entidad.Id,
        Tipo = entidad.Tipo,
        Titulo = entidad.Titulo,
        ClaveDirecta = entidad.ClaveDirecta,
        CreadoPorUsuarioId = entidad.CreadoPorUsuarioId,
        UltimoMensajeId = entidad.UltimoMensajeId,
        UltimoMensajeEn = entidad.UltimoMensajeEn,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn
    };
}
