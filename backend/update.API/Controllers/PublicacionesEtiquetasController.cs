using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de publicaciones_etiquetas; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/publicaciones_etiquetas")]
public sealed class PublicacionesEtiquetasController(ServicioCrud<PublicacionEtiqueta> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaPublicacionEtiquetaDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{publicacionId:guid}/{etiquetaId:int}")]
    public async Task<ActionResult<RespuestaPublicacionEtiquetaDto>> Obtener(Guid publicacionId, int etiquetaId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([publicacionId, etiquetaId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaPublicacionEtiquetaDto>> Crear([FromBody] CrearPublicacionEtiquetaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new PublicacionEtiqueta
        {
            PublicacionId = entrada.PublicacionId,
            EtiquetaId = entrada.EtiquetaId
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { publicacionId = entidad.PublicacionId, etiquetaId = entidad.EtiquetaId }, ARespuesta(entidad));
    }

    [HttpPut("{publicacionId:guid}/{etiquetaId:int}")]
    public async Task<IActionResult> Actualizar(Guid publicacionId, int etiquetaId, [FromBody] ActualizarPublicacionEtiquetaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([publicacionId, etiquetaId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });

        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{publicacionId:guid}/{etiquetaId:int}")]
    public async Task<IActionResult> Eliminar(Guid publicacionId, int etiquetaId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([publicacionId, etiquetaId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaPublicacionEtiquetaDto ARespuesta(PublicacionEtiqueta entidad) => new()
    {
        PublicacionId = entidad.PublicacionId,
        EtiquetaId = entidad.EtiquetaId
    };
}
