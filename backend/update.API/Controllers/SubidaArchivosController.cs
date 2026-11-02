using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;

namespace update.API.Controllers;

[ApiController]
[Authorize]
[Route("api/archivos")]
public sealed class SubidaArchivosController(ServicioSubidaArchivos servicio) : ControllerBase
{
    private const long LimitePeticionBytes = ServicioSubidaArchivos.TamanoMaximoBytes + 1024 * 1024;

    /// <summary>Sube una imagen. El campo "proposito" es "foto_perfil" (por defecto) o "publicacion".</summary>
    [HttpPost("subir")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(LimitePeticionBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = LimitePeticionBytes)]
    [ProducesResponseType<RespuestaArchivoSubidoDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<RespuestaArchivoSubidoDto>> Subir(
        [Required(ErrorMessage = "Elige una imagen.")] IFormFile archivo,
        [FromForm] string? proposito,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        await using var contenido = archivo.OpenReadStream();
        var guardado = await servicio.SubirImagenAsync(
            usuarioId, proposito, contenido, archivo.FileName, archivo.Length, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, new RespuestaArchivoSubidoDto
        {
            Id = guardado.Id,
            Url = $"{Request.Scheme}://{Request.Host}/uploads/{guardado.ClaveObjeto}",
            TipoMime = guardado.TipoMime,
            TamanoBytes = guardado.TamanoBytes
        });
    }
}
