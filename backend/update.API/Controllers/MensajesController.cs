using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de mensajes; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/mensajes")]
public sealed class MensajesController(ServicioCrud<Mensaje> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaMensajeDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaMensajeDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaMensajeDto>> Crear([FromBody] CrearMensajeDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Mensaje
        {
            ConversacionId = entrada.ConversacionId,
            PerfilRemitenteId = entrada.PerfilRemitenteId,
            UsuarioRemitenteId = entrada.UsuarioRemitenteId,
            Tipo = entrada.Tipo,
            Contenido = entrada.Contenido,
            ProductoId = entrada.ProductoId,
            SolicitudCotizacionId = entrada.SolicitudCotizacionId,
            RespondeAMensajeId = entrada.RespondeAMensajeId
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarMensajeDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.ConversacionId = entrada.ConversacionId;
        entidad.PerfilRemitenteId = entrada.PerfilRemitenteId;
        entidad.UsuarioRemitenteId = entrada.UsuarioRemitenteId;
        entidad.Tipo = entrada.Tipo;
        entidad.Contenido = entrada.Contenido;
        entidad.ProductoId = entrada.ProductoId;
        entidad.SolicitudCotizacionId = entrada.SolicitudCotizacionId;
        entidad.RespondeAMensajeId = entrada.RespondeAMensajeId;
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

    private static RespuestaMensajeDto ARespuesta(Mensaje entidad) => new()
    {
        Id = entidad.Id,
        ConversacionId = entidad.ConversacionId,
        PerfilRemitenteId = entidad.PerfilRemitenteId,
        UsuarioRemitenteId = entidad.UsuarioRemitenteId,
        Tipo = entidad.Tipo,
        Contenido = entidad.Contenido,
        ProductoId = entidad.ProductoId,
        SolicitudCotizacionId = entidad.SolicitudCotizacionId,
        RespondeAMensajeId = entidad.RespondeAMensajeId,
        CreadoEn = entidad.CreadoEn,
        EditadoEn = entidad.EditadoEn,
        EliminadoEn = entidad.EliminadoEn
    };
}
