using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;

namespace update.API.Controllers;

/// <summary>Publicaciones del feed: cualquiera con sesión las ve; solo su autor las edita o elimina.</summary>
[ApiController]
[Authorize]
[Route("api/feed")]
public sealed class FeedController(ServicioFeed servicio) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<RespuestaPublicacionFeedDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RespuestaPublicacionFeedDto>>> Listar(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 20,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var resultado = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(resultado.Select(p => ARespuesta(p, usuarioId)).ToList());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<RespuestaPublicacionFeedDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespuestaPublicacionFeedDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var resultado = await servicio.ObtenerAsync(id, cancellationToken);
        return resultado is null ? NoEncontrada() : Ok(ARespuesta(resultado, usuarioId));
    }

    [HttpPost]
    [ProducesResponseType<RespuestaPublicacionFeedDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RespuestaPublicacionFeedDto>> Crear(
        [FromBody] GuardarPublicacionDto entrada,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var resultado = await servicio.CrearAsync(usuarioId, entrada, cancellationToken);
        if (resultado is null) return NotFound();
        return CreatedAtAction(nameof(Obtener), new { id = resultado.Publicacion.Id }, ARespuesta(resultado, usuarioId));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<RespuestaPublicacionFeedDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespuestaPublicacionFeedDto>> Actualizar(
        Guid id,
        [FromBody] GuardarPublicacionDto entrada,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        try
        {
            var resultado = await servicio.ActualizarAsync(usuarioId, id, entrada, cancellationToken);
            return resultado is null ? NoEncontrada() : Ok(ARespuesta(resultado, usuarioId));
        }
        catch (PublicacionAjenaException ex)
        {
            return Prohibido(ex);
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        try
        {
            return await servicio.EliminarAsync(usuarioId, id, cancellationToken) ? NoContent() : NoEncontrada();
        }
        catch (PublicacionAjenaException ex)
        {
            return Prohibido(ex);
        }
    }

    private ObjectResult NoEncontrada() => NotFound(new ProblemDetails
    {
        Status = StatusCodes.Status404NotFound,
        Title = "La publicación no existe o fue eliminada."
    });

    private ObjectResult Prohibido(PublicacionAjenaException ex) => StatusCode(StatusCodes.Status403Forbidden, new ProblemDetails
    {
        Status = StatusCodes.Status403Forbidden,
        Title = ex.Message
    });

    private RespuestaPublicacionFeedDto ARespuesta(PublicacionFeed resultado, Guid usuarioId) => new()
    {
        Id = resultado.Publicacion.Id,
        Contenido = resultado.Publicacion.Contenido,
        ImagenArchivoId = resultado.ImagenArchivoId,
        ImagenUrl = UrlArchivo(resultado.ClaveImagen),
        Autor = new AutorPublicacionDto
        {
            NombreUsuario = resultado.Autor.NombreUsuario,
            NombreVisible = resultado.Autor.NombreVisible,
            FotoUrl = UrlArchivo(resultado.ClaveFotoAutor)
        },
        EsMia = resultado.Publicacion.CreadoPorUsuarioId == usuarioId,
        CreadoEn = resultado.Publicacion.CreadoEn,
        EditadoEn = resultado.Publicacion.EditadoEn
    };

    private string? UrlArchivo(string? clave) => clave is null ? null : $"{Request.Scheme}://{Request.Host}/uploads/{clave}";
}
