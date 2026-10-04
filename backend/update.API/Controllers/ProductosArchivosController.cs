using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de productos_archivos; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/productos_archivos")]
public sealed class ProductosArchivosController(ServicioCrud<ProductoArchivo> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaProductoArchivoDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{productoId:guid}/{archivoId:guid}")]
    public async Task<ActionResult<RespuestaProductoArchivoDto>> Obtener(Guid productoId, Guid archivoId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([productoId, archivoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaProductoArchivoDto>> Crear([FromBody] CrearProductoArchivoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new ProductoArchivo
        {
            ProductoId = entrada.ProductoId,
            ArchivoId = entrada.ArchivoId,
            Posicion = entrada.Posicion,
            TextoAlternativo = entrada.TextoAlternativo
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { productoId = entidad.ProductoId, archivoId = entidad.ArchivoId }, ARespuesta(entidad));
    }

    [HttpPut("{productoId:guid}/{archivoId:guid}")]
    public async Task<IActionResult> Actualizar(Guid productoId, Guid archivoId, [FromBody] ActualizarProductoArchivoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([productoId, archivoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.Posicion = entrada.Posicion;
        entidad.TextoAlternativo = entrada.TextoAlternativo;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{productoId:guid}/{archivoId:guid}")]
    public async Task<IActionResult> Eliminar(Guid productoId, Guid archivoId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([productoId, archivoId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaProductoArchivoDto ARespuesta(ProductoArchivo entidad) => new()
    {
        ProductoId = entidad.ProductoId,
        ArchivoId = entidad.ArchivoId,
        Posicion = entidad.Posicion,
        TextoAlternativo = entidad.TextoAlternativo
    };
}
