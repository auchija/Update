using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;

namespace update.API.Controllers;

[ApiController]
[Authorize]
[Route("api/archivos")]
public sealed class SubidaArchivosController(ServicioSubidaArchivos servicio, FirmaArchivos firmas) : ControllerBase
{
    private const long LimitePeticionBytes = ServicioSubidaArchivos.TamanoMaximoBytes + 1024 * 1024;

    /// <summary>Sube un archivo de perfil, publicacion, producto o emprendimiento.</summary>
    [HttpPost("subir")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(LimitePeticionBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = LimitePeticionBytes)]
    [ProducesResponseType<RespuestaArchivoSubidoDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
            Url = UrlDe(guardado),
            TipoMime = guardado.TipoMime,
            TamanoBytes = guardado.TamanoBytes
        });
    }

    /// <summary>Historial paginado de los archivos que subió el usuario con sesión.</summary>
    [HttpGet("mios")]
    [ProducesResponseType<IReadOnlyList<RespuestaArchivoHistorialDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<RespuestaArchivoHistorialDto>>> Historial(int pagina = 1, int tamanoPagina = 20, CancellationToken ct = default)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuario)) return Unauthorized();
        var lista = await servicio.HistorialAsync(usuario, pagina, tamanoPagina, ct);
        return Ok(lista.Select(a => new RespuestaArchivoHistorialDto
        {
            Id = a.Id,
            NombreOriginal = a.NombreOriginal,
            TamanoBytes = a.TamanoBytes,
            TipoMime = a.TipoMime,
            CreadoEn = a.CreadoEn,
            SubidoPorUsuarioId = a.SubidoPorUsuarioId,
            Proposito = a.Proposito,
            Url = UrlDe(a)
        }).ToList());
    }

    /// <summary>Muestra o descarga un archivo. Los de publicaciones de cuentas privadas exigen un enlace firmado vigente.</summary>
    [AllowAnonymous]
    [HttpGet("/uploads/{clave}")]
    [ProducesResponseType<FileContentResult>(StatusCodes.Status200OK, "image/jpeg", "image/png", "image/webp", "application/pdf")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Ver(string clave, [FromQuery] long? exp, [FromQuery] string? firma, CancellationToken ct)
    {
        var resultado = await servicio.AbrirAsync(clave, exp, firma, ct);
        if (resultado is null) return NotFound();
        var (archivo, datos, restringido) = resultado.Value;
        Response.Headers.XContentTypeOptions = "nosniff";
        Response.Headers.ContentSecurityPolicy = "sandbox";
        Response.Headers.CacheControl = restringido ? "private, max-age=900" : "private, max-age=31536000, immutable";
        var etiqueta = string.IsNullOrEmpty(archivo.ChecksumSha256) ? null : new EntityTagHeaderValue($"\"{archivo.ChecksumSha256}\"");
        return archivo.TipoMime == "application/pdf"
            ? File(datos, "application/pdf", archivo.NombreOriginal ?? "archivo.pdf", null, etiqueta!)
            : File(datos, archivo.TipoMime, null, etiqueta!);
    }

    /// <summary>Borra un archivo propio del almacenamiento y su registro. 409 si sigue en uso en un perfil, publicación o producto.</summary>
    [HttpDelete("mios/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuario)) return Unauthorized();
        await servicio.EliminarPropioAsync(usuario, id, ct);
        return NoContent();
    }

    /// <summary>Quita un archivo de un producto propio; el archivo sigue en el historial.</summary>
    [HttpDelete("/api/mis-productos/{productoId:guid}/archivos/{archivoId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DesvincularProducto(Guid productoId, Guid archivoId, CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuario)) return Unauthorized();
        await servicio.DesvincularProductoAsync(usuario, productoId, archivoId, ct);
        return NoContent();
    }

    /// <summary>Adjunta a un producto propio un archivo subido con proposito=producto.</summary>
    [HttpPut("/api/mis-productos/{productoId:guid}/archivos/{archivoId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdjuntarProducto(Guid productoId, Guid archivoId, CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuario)) return Unauthorized();
        await servicio.AdjuntarProductoAsync(usuario, productoId, archivoId, ct);
        return NoContent();
    }

    private string UrlDe(Archivo archivo) => archivo.Proposito == PropositoArchivo.Publicacion
        ? Request.Firmada(archivo.ClaveObjeto, firmas)!
        : Request.Publica(archivo.ClaveObjeto)!;
}
