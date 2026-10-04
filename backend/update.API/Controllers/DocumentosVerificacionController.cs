using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de documentos_verificacion; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/documentos_verificacion")]
public sealed class DocumentosVerificacionController(ServicioCrud<DocumentoVerificacion> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaDocumentoVerificacionDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{verificacionId:guid}/{archivoId:guid}")]
    public async Task<ActionResult<RespuestaDocumentoVerificacionDto>> Obtener(Guid verificacionId, Guid archivoId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([verificacionId, archivoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaDocumentoVerificacionDto>> Crear([FromBody] CrearDocumentoVerificacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new DocumentoVerificacion
        {
            VerificacionId = entrada.VerificacionId,
            ArchivoId = entrada.ArchivoId,
            TipoDocumento = entrada.TipoDocumento
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { verificacionId = entidad.VerificacionId, archivoId = entidad.ArchivoId }, ARespuesta(entidad));
    }

    [HttpPut("{verificacionId:guid}/{archivoId:guid}")]
    public async Task<IActionResult> Actualizar(Guid verificacionId, Guid archivoId, [FromBody] ActualizarDocumentoVerificacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([verificacionId, archivoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.TipoDocumento = entrada.TipoDocumento;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{verificacionId:guid}/{archivoId:guid}")]
    public async Task<IActionResult> Eliminar(Guid verificacionId, Guid archivoId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([verificacionId, archivoId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaDocumentoVerificacionDto ARespuesta(DocumentoVerificacion entidad) => new()
    {
        VerificacionId = entidad.VerificacionId,
        ArchivoId = entidad.ArchivoId,
        TipoDocumento = entidad.TipoDocumento
    };
}
