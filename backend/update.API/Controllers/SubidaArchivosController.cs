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

    [HttpPost("subir")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(LimitePeticionBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = LimitePeticionBytes)]
    [ProducesResponseType<RespuestaArchivoSubidoDto>(StatusCodes.Status201Created)]
    public async Task<ActionResult<RespuestaArchivoSubidoDto>> Subir(IFormFile archivo, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        await using var contenido = archivo.OpenReadStream();
        var guardado = await servicio.SubirFotoPerfilAsync(
            usuarioId, contenido, archivo.FileName, archivo.Length, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, new RespuestaArchivoSubidoDto
        {
            Id = guardado.Id,
            Url = $"{Request.Scheme}://{Request.Host}/uploads/{guardado.ClaveObjeto}",
            TipoMime = guardado.TipoMime,
            TamanoBytes = guardado.TamanoBytes
        });
    }
}
