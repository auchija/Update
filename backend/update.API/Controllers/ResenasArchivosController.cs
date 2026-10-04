using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de resenas_archivos; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/resenas_archivos")]
public sealed class ResenasArchivosController(ServicioCrud<ResenaArchivo> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaResenaArchivoDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{resenaId:guid}/{archivoId:guid}")]
    public async Task<ActionResult<RespuestaResenaArchivoDto>> Obtener(Guid resenaId, Guid archivoId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([resenaId, archivoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaResenaArchivoDto>> Crear([FromBody] CrearResenaArchivoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new ResenaArchivo
        {
            ResenaId = entrada.ResenaId,
            ArchivoId = entrada.ArchivoId,
            Posicion = entrada.Posicion
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { resenaId = entidad.ResenaId, archivoId = entidad.ArchivoId }, ARespuesta(entidad));
    }

    [HttpPut("{resenaId:guid}/{archivoId:guid}")]
    public async Task<IActionResult> Actualizar(Guid resenaId, Guid archivoId, [FromBody] ActualizarResenaArchivoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([resenaId, archivoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.Posicion = entrada.Posicion;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{resenaId:guid}/{archivoId:guid}")]
    public async Task<IActionResult> Eliminar(Guid resenaId, Guid archivoId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([resenaId, archivoId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaResenaArchivoDto ARespuesta(ResenaArchivo entidad) => new()
    {
        ResenaId = entidad.ResenaId,
        ArchivoId = entidad.ArchivoId,
        Posicion = entidad.Posicion
    };
}
