using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de categorias; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/categorias")]
public sealed class CategoriasController(ServicioCrud<Categoria> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaCategoriaDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RespuestaCategoriaDto>> Obtener(int id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaCategoriaDto>> Crear([FromBody] CrearCategoriaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Categoria
        {
            PadreId = entrada.PadreId,
            Slug = entrada.Slug,
            Nombre = entrada.Nombre,
            Icono = entrada.Icono,
            Orden = entrada.Orden,
            Activo = entrada.Activo
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarCategoriaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.PadreId = entrada.PadreId;
        entidad.Slug = entrada.Slug;
        entidad.Nombre = entrada.Nombre;
        entidad.Icono = entrada.Icono;
        entidad.Orden = entrada.Orden;
        entidad.Activo = entrada.Activo;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([id], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaCategoriaDto ARespuesta(Categoria entidad) => new()
    {
        Id = entidad.Id,
        PadreId = entidad.PadreId,
        Slug = entidad.Slug,
        Nombre = entidad.Nombre,
        Icono = entidad.Icono,
        Orden = entidad.Orden,
        Activo = entidad.Activo
    };
}
