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

    /// <summary>Sube un archivo de perfil, publicacion, producto o emprendimiento.</summary>
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

    [HttpGet("mios")]
    public async Task<IActionResult> Historial(int pagina = 1, int tamanoPagina = 20, CancellationToken ct = default)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuario)) return Unauthorized();
        var lista = await servicio.HistorialAsync(usuario, pagina, tamanoPagina, ct);
        return Ok(lista.Select(a => new { a.Id, a.NombreOriginal, a.TamanoBytes, a.TipoMime, a.CreadoEn,
            a.SubidoPorUsuarioId, a.Proposito, url = $"/uploads/{a.ClaveObjeto}" }));
    }

    [AllowAnonymous]
    [HttpGet("/uploads/{clave}")]
    public async Task<IActionResult> Ver(string clave, CancellationToken ct)
    {
        var resultado = await servicio.AbrirAsync(clave, ct);
        if (resultado is null) return NotFound();
        var (archivo, datos) = resultado.Value;
        Response.Headers.XContentTypeOptions = "nosniff";
        Response.Headers.ContentSecurityPolicy = "sandbox";
        return archivo.TipoMime == "application/pdf"
            ? File(datos, "application/pdf", archivo.NombreOriginal ?? "archivo.pdf")
            : File(datos, archivo.TipoMime);
    }

    [HttpDelete("mios/{id:guid}")]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuario)) return Unauthorized();
        return StatusCode(await servicio.EliminarPropioAsync(usuario, id, ct));
    }
    [HttpDelete("/api/mis-productos/{productoId:guid}/archivos/{archivoId:guid}")]
    public async Task<IActionResult> DesvincularProducto(Guid productoId, Guid archivoId, CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuario)) return Unauthorized();
        return StatusCode(await servicio.DesvincularProductoAsync(usuario, productoId, archivoId, ct));
    }

    [HttpPut("/api/mis-productos/{productoId:guid}/archivos/{archivoId:guid}")]
    public async Task<IActionResult> AdjuntarProducto(Guid productoId, Guid archivoId, CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuario)) return Unauthorized();
        return StatusCode(await servicio.AdjuntarProductoAsync(usuario, productoId, archivoId, ct));
    }
}
