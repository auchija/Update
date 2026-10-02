using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de cotizacion_items; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/cotizacion_items")]
public sealed class CotizacionItemsController(ServicioCrud<CotizacionItem> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaCotizacionItemDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaCotizacionItemDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaCotizacionItemDto>> Crear([FromBody] CrearCotizacionItemDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new CotizacionItem
        {
            CotizacionId = entrada.CotizacionId,
            ProductoId = entrada.ProductoId,
            Descripcion = entrada.Descripcion,
            Cantidad = entrada.Cantidad,
            PrecioUnitario = entrada.PrecioUnitario
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarCotizacionItemDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.CotizacionId = entrada.CotizacionId;
        entidad.ProductoId = entrada.ProductoId;
        entidad.Descripcion = entrada.Descripcion;
        entidad.Cantidad = entrada.Cantidad;
        entidad.PrecioUnitario = entrada.PrecioUnitario;
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

    private static RespuestaCotizacionItemDto ARespuesta(CotizacionItem entidad) => new()
    {
        Id = entidad.Id,
        CotizacionId = entidad.CotizacionId,
        ProductoId = entidad.ProductoId,
        Descripcion = entidad.Descripcion,
        Cantidad = entidad.Cantidad,
        PrecioUnitario = entidad.PrecioUnitario,
        TotalLinea = entidad.TotalLinea
    };
}
