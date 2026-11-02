using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Exceptions;

namespace update.API.Controllers;

[ApiController]
[Authorize]
[Route("api/feed")]
public sealed class FeedController(ServicioPublicaciones servicio) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar(
        [FromQuery, Range(1, 100000)] int pagina = 1,
        [FromQuery, Range(1, 100)] int tamanoPagina = 20,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();
        return Ok(await servicio.ListarAsync(usuarioId, pagina, tamanoPagina, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Crear(GuardarPublicacionFeedDto entrada, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();
        try
        {
            var respuesta = await servicio.CrearAsync(usuarioId, entrada, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, respuesta);
        }
        catch (ExcepcionDominio ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, GuardarPublicacionFeedDto entrada, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();
        try { return Ok(await servicio.ActualizarAsync(id, usuarioId, entrada, cancellationToken)); }
        catch (PublicacionNoEncontradaException) { return NotFound(new { message = "La publicacion ya no existe." }); }
        catch (PublicacionAjenaException) { return Forbid(); }
        catch (ExcepcionDominio ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();
        try
        {
            await servicio.EliminarAsync(id, usuarioId, cancellationToken);
            return NoContent();
        }
        catch (PublicacionNoEncontradaException) { return NotFound(new { message = "La publicacion ya no existe." }); }
        catch (PublicacionAjenaException) { return Forbid(); }
    }
}
