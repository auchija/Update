using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de notificaciones; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/notificaciones")]
public sealed class NotificacionesController(ServicioCrud<Notificacion> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaNotificacionDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaNotificacionDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaNotificacionDto>> Crear([FromBody] CrearNotificacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Notificacion
        {
            UsuarioDestinatarioId = entrada.UsuarioDestinatarioId,
            Tipo = entrada.Tipo,
            PerfilActorId = entrada.PerfilActorId,
            EntidadTipo = entrada.EntidadTipo,
            EntidadId = entrada.EntidadId,
            ClaveGrupo = entrada.ClaveGrupo,
            Datos = entrada.Datos,
            LeidoEn = entrada.LeidoEn
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarNotificacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.UsuarioDestinatarioId = entrada.UsuarioDestinatarioId;
        entidad.Tipo = entrada.Tipo;
        entidad.PerfilActorId = entrada.PerfilActorId;
        entidad.EntidadTipo = entrada.EntidadTipo;
        entidad.EntidadId = entrada.EntidadId;
        entidad.ClaveGrupo = entrada.ClaveGrupo;
        entidad.Datos = entrada.Datos;
        entidad.LeidoEn = entrada.LeidoEn;
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

    private static RespuestaNotificacionDto ARespuesta(Notificacion entidad) => new()
    {
        Id = entidad.Id,
        UsuarioDestinatarioId = entidad.UsuarioDestinatarioId,
        Tipo = entidad.Tipo,
        PerfilActorId = entidad.PerfilActorId,
        EntidadTipo = entidad.EntidadTipo,
        EntidadId = entidad.EntidadId,
        ClaveGrupo = entidad.ClaveGrupo,
        Datos = entidad.Datos,
        LeidoEn = entidad.LeidoEn,
        CreadoEn = entidad.CreadoEn
    };
}
