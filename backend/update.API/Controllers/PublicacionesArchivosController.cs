using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de publicaciones_archivos; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/publicaciones_archivos")]
public sealed class PublicacionesArchivosController(ServicioCrud<PublicacionArchivo> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaPublicacionArchivoDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{publicacionId:guid}/{archivoId:guid}")]
    public async Task<ActionResult<RespuestaPublicacionArchivoDto>> Obtener(Guid publicacionId, Guid archivoId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([publicacionId, archivoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaPublicacionArchivoDto>> Crear([FromBody] CrearPublicacionArchivoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new PublicacionArchivo
        {
            PublicacionId = entrada.PublicacionId,
            ArchivoId = entrada.ArchivoId,
            Posicion = entrada.Posicion,
            TextoAlternativo = entrada.TextoAlternativo
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { publicacionId = entidad.PublicacionId, archivoId = entidad.ArchivoId }, ARespuesta(entidad));
    }

    [HttpPut("{publicacionId:guid}/{archivoId:guid}")]
    public async Task<IActionResult> Actualizar(Guid publicacionId, Guid archivoId, [FromBody] ActualizarPublicacionArchivoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([publicacionId, archivoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.Posicion = entrada.Posicion;
        entidad.TextoAlternativo = entrada.TextoAlternativo;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{publicacionId:guid}/{archivoId:guid}")]
    public async Task<IActionResult> Eliminar(Guid publicacionId, Guid archivoId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([publicacionId, archivoId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaPublicacionArchivoDto ARespuesta(PublicacionArchivo entidad) => new()
    {
        PublicacionId = entidad.PublicacionId,
        ArchivoId = entidad.ArchivoId,
        Posicion = entidad.Posicion,
        TextoAlternativo = entidad.TextoAlternativo
    };
}
