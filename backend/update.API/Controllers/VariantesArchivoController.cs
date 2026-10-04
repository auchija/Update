using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de variantes_archivo; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/variantes_archivo")]
public sealed class VariantesArchivoController(ServicioCrud<VarianteArchivo> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaVarianteArchivoDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{archivoId:guid}/{variante}")]
    public async Task<ActionResult<RespuestaVarianteArchivoDto>> Obtener(Guid archivoId, string variante, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([archivoId, variante], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaVarianteArchivoDto>> Crear([FromBody] CrearVarianteArchivoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new VarianteArchivo
        {
            ArchivoId = entrada.ArchivoId,
            Variante = entrada.Variante,
            ClaveObjeto = entrada.ClaveObjeto,
            TipoMime = entrada.TipoMime,
            Ancho = entrada.Ancho,
            Alto = entrada.Alto,
            TamanoBytes = entrada.TamanoBytes
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { archivoId = entidad.ArchivoId, variante = entidad.Variante }, ARespuesta(entidad));
    }

    [HttpPut("{archivoId:guid}/{variante}")]
    public async Task<IActionResult> Actualizar(Guid archivoId, string variante, [FromBody] ActualizarVarianteArchivoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([archivoId, variante], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.ClaveObjeto = entrada.ClaveObjeto;
        entidad.TipoMime = entrada.TipoMime;
        entidad.Ancho = entrada.Ancho;
        entidad.Alto = entrada.Alto;
        entidad.TamanoBytes = entrada.TamanoBytes;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{archivoId:guid}/{variante}")]
    public async Task<IActionResult> Eliminar(Guid archivoId, string variante, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([archivoId, variante], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaVarianteArchivoDto ARespuesta(VarianteArchivo entidad) => new()
    {
        ArchivoId = entidad.ArchivoId,
        Variante = entidad.Variante,
        ClaveObjeto = entidad.ClaveObjeto,
        TipoMime = entidad.TipoMime,
        Ancho = entidad.Ancho,
        Alto = entidad.Alto,
        TamanoBytes = entidad.TamanoBytes
    };
}
