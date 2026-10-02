using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de productos_etiquetas; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/productos_etiquetas")]
public sealed class ProductosEtiquetasController(ServicioCrud<ProductoEtiqueta> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaProductoEtiquetaDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{productoId:guid}/{etiquetaId:int}")]
    public async Task<ActionResult<RespuestaProductoEtiquetaDto>> Obtener(Guid productoId, int etiquetaId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([productoId, etiquetaId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaProductoEtiquetaDto>> Crear([FromBody] CrearProductoEtiquetaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new ProductoEtiqueta
        {
            ProductoId = entrada.ProductoId,
            EtiquetaId = entrada.EtiquetaId
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { productoId = entidad.ProductoId, etiquetaId = entidad.EtiquetaId }, ARespuesta(entidad));
    }

    [HttpPut("{productoId:guid}/{etiquetaId:int}")]
    public async Task<IActionResult> Actualizar(Guid productoId, int etiquetaId, [FromBody] ActualizarProductoEtiquetaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([productoId, etiquetaId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });

        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{productoId:guid}/{etiquetaId:int}")]
    public async Task<IActionResult> Eliminar(Guid productoId, int etiquetaId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([productoId, etiquetaId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaProductoEtiquetaDto ARespuesta(ProductoEtiqueta entidad) => new()
    {
        ProductoId = entidad.ProductoId,
        EtiquetaId = entidad.EtiquetaId
    };
}
