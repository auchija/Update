using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de solicitud_items; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/solicitud_items")]
public sealed class SolicitudItemsController(ServicioCrud<SolicitudItem> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaSolicitudItemDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaSolicitudItemDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaSolicitudItemDto>> Crear([FromBody] CrearSolicitudItemDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new SolicitudItem
        {
            SolicitudCotizacionId = entrada.SolicitudCotizacionId,
            ProductoId = entrada.ProductoId,
            Descripcion = entrada.Descripcion,
            Cantidad = entrada.Cantidad,
            Notas = entrada.Notas
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarSolicitudItemDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.SolicitudCotizacionId = entrada.SolicitudCotizacionId;
        entidad.ProductoId = entrada.ProductoId;
        entidad.Descripcion = entrada.Descripcion;
        entidad.Cantidad = entrada.Cantidad;
        entidad.Notas = entrada.Notas;
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

    private static RespuestaSolicitudItemDto ARespuesta(SolicitudItem entidad) => new()
    {
        Id = entidad.Id,
        SolicitudCotizacionId = entidad.SolicitudCotizacionId,
        ProductoId = entidad.ProductoId,
        Descripcion = entidad.Descripcion,
        Cantidad = entidad.Cantidad,
        Notas = entidad.Notas
    };
}
