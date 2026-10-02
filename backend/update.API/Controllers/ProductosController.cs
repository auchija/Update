using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de productos; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/productos")]
public sealed class ProductosController(ServicioCrud<Producto> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaProductoDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaProductoDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaProductoDto>> Crear([FromBody] CrearProductoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Producto
        {
            EmprendimientoId = entrada.EmprendimientoId,
            CreadoPorUsuarioId = entrada.CreadoPorUsuarioId,
            Tipo = entrada.Tipo,
            Titulo = entrada.Titulo,
            Slug = entrada.Slug,
            Descripcion = entrada.Descripcion,
            CategoriaId = entrada.CategoriaId,
            TipoPrecio = entrada.TipoPrecio,
            Precio = entrada.Precio,
            MonedaCodigo = entrada.MonedaCodigo,
            EstadoInventario = entrada.EstadoInventario,
            CantidadInventario = entrada.CantidadInventario,
            CiudadId = entrada.CiudadId,
            Estado = entrada.Estado,
            CalificacionPromedio = entrada.CalificacionPromedio,
            TotalCalificaciones = entrada.TotalCalificaciones,
            TotalGuardados = entrada.TotalGuardados,
            PublicadoEn = entrada.PublicadoEn
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarProductoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.CreadoPorUsuarioId = entrada.CreadoPorUsuarioId;
        entidad.Tipo = entrada.Tipo;
        entidad.Titulo = entrada.Titulo;
        entidad.Slug = entrada.Slug;
        entidad.Descripcion = entrada.Descripcion;
        entidad.CategoriaId = entrada.CategoriaId;
        entidad.TipoPrecio = entrada.TipoPrecio;
        entidad.Precio = entrada.Precio;
        entidad.MonedaCodigo = entrada.MonedaCodigo;
        entidad.EstadoInventario = entrada.EstadoInventario;
        entidad.CantidadInventario = entrada.CantidadInventario;
        entidad.CiudadId = entrada.CiudadId;
        entidad.Estado = entrada.Estado;
        entidad.CalificacionPromedio = entrada.CalificacionPromedio;
        entidad.TotalCalificaciones = entrada.TotalCalificaciones;
        entidad.TotalGuardados = entrada.TotalGuardados;
        entidad.PublicadoEn = entrada.PublicadoEn;
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

    private static RespuestaProductoDto ARespuesta(Producto entidad) => new()
    {
        Id = entidad.Id,
        EmprendimientoId = entidad.EmprendimientoId,
        CreadoPorUsuarioId = entidad.CreadoPorUsuarioId,
        Tipo = entidad.Tipo,
        Titulo = entidad.Titulo,
        Slug = entidad.Slug,
        Descripcion = entidad.Descripcion,
        CategoriaId = entidad.CategoriaId,
        TipoPrecio = entidad.TipoPrecio,
        Precio = entidad.Precio,
        MonedaCodigo = entidad.MonedaCodigo,
        EstadoInventario = entidad.EstadoInventario,
        CantidadInventario = entidad.CantidadInventario,
        CiudadId = entidad.CiudadId,
        Estado = entidad.Estado,
        CalificacionPromedio = entidad.CalificacionPromedio,
        TotalCalificaciones = entidad.TotalCalificaciones,
        TotalGuardados = entidad.TotalGuardados,
        PublicadoEn = entidad.PublicadoEn,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn,
        EliminadoEn = entidad.EliminadoEn
    };
}
