using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;

namespace update.API.Controllers;

/// <summary>Publicaciones del feed: cualquiera con sesión las ve; solo su autor las edita o elimina.</summary>
[ApiController]
[Authorize]
[Route("api/feed")]
public sealed class FeedController(ServicioFeed servicio, ServicioComunidad comunidad, FirmaArchivos firmas) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Lista las publicaciones que puedo ver, de la más nueva a la más antigua.")]
    [ProducesResponseType<IReadOnlyList<RespuestaPublicacionFeedDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RespuestaPublicacionFeedDto>>> Listar(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 20,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var resultado = await servicio.ListarAsync(usuarioId, pagina, tamanoPagina, cancellationToken);
        return Ok(resultado.Select(p => ARespuesta(p, usuarioId)).ToList());
    }

    /// <summary>Historial de publicaciones propias (incluidas las no públicas), de la más nueva a la más antigua.</summary>
    [HttpGet("mias")]
    [ProducesResponseType<IReadOnlyList<RespuestaPublicacionFeedDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<RespuestaPublicacionFeedDto>>> MisPublicaciones(int pagina = 1, int tamanoPagina = 20, CancellationToken ct = default)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuario)) return Unauthorized();
        var resultado = await servicio.ListarAsync(usuario, pagina, tamanoPagina, ct, soloMias: true);
        return Ok(resultado.Select(p => ARespuesta(p, usuario)).ToList());
    }

    [HttpGet("{id:guid}")]
    [EndpointSummary("Devuelve una publicación.")]
    [ProducesResponseType<RespuestaPublicacionFeedDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespuestaPublicacionFeedDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var resultado = await servicio.ObtenerAsync(id, usuarioId, cancellationToken);
        return resultado is null ? NoEncontrada() : Ok(ARespuesta(resultado, usuarioId));
    }

    [HttpPost]
    [EndpointSummary("Publica un texto con un archivo opcional.")]
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
    [EndpointSummary("Edita una publicación propia.")]
    [ProducesResponseType<RespuestaPublicacionFeedDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespuestaPublicacionFeedDto>> Actualizar(
        Guid id,
        [FromBody] GuardarPublicacionDto entrada,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var resultado = await servicio.ActualizarAsync(usuarioId, id, entrada, cancellationToken);
        return resultado is null ? NoEncontrada() : Ok(ARespuesta(resultado, usuarioId));
    }

    [HttpDelete("{id:guid}")]
    [EndpointSummary("Elimina (borrado lógico) una publicación propia.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        return await servicio.EliminarAsync(usuarioId, id, cancellationToken) ? NoContent() : NoEncontrada();
    }

    private ObjectResult NoEncontrada() => NotFound(new ProblemDetails
    {
        Status = StatusCodes.Status404NotFound,
        Title = "La publicación no existe o fue eliminada."
    });

    /// <summary>Da "me gusta" a la publicación. Repetirlo no suma otra vez.</summary>
    [HttpPost("{id:guid}/me-gusta")]
    [ProducesResponseType<EstadoMeGustaDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public Task<ActionResult<EstadoMeGustaDto>> DarMeGusta(Guid id, CancellationToken cancellationToken)
        => MarcarMeGusta(id, true, cancellationToken);

    /// <summary>Quita el "me gusta". Repetirlo no resta otra vez.</summary>
    [HttpDelete("{id:guid}/me-gusta")]
    [ProducesResponseType<EstadoMeGustaDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public Task<ActionResult<EstadoMeGustaDto>> QuitarMeGusta(Guid id, CancellationToken cancellationToken)
        => MarcarMeGusta(id, false, cancellationToken);

    /// <summary>Comentarios de la publicación, del más antiguo al más nuevo.</summary>
    [HttpGet("{id:guid}/comentarios")]
    [ProducesResponseType<IReadOnlyList<RespuestaComentarioFeedDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<RespuestaComentarioFeedDto>>> ListarComentarios(
        Guid id,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 20,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var resultado = await comunidad.ListarComentariosAsync(usuarioId, id, pagina, tamanoPagina, cancellationToken);
        return resultado is null ? NoEncontrada() : Ok(resultado.Select(c => MapeoComunidad.Comentario(c, UrlArchivo)).ToList());
    }

    [HttpPost("{id:guid}/comentarios")]
    [EndpointSummary("Comenta una publicación.")]
    [ProducesResponseType<RespuestaComentarioFeedDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RespuestaComentarioFeedDto>> Comentar(
        Guid id,
        [FromBody] ComentarDto entrada,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var resultado = await comunidad.ComentarAsync(usuarioId, id, entrada, cancellationToken);
        return resultado is null
            ? NoEncontrada()
            : StatusCode(StatusCodes.Status201Created, MapeoComunidad.Comentario(resultado, UrlArchivo));
    }

    /// <summary>Borra un comentario propio, o cualquiera de una publicación propia.</summary>
    [HttpDelete("{id:guid}/comentarios/{comentarioId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EliminarComentario(Guid id, Guid comentarioId, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        return await comunidad.EliminarComentarioAsync(usuarioId, id, comentarioId, cancellationToken)
            ? NoContent()
            : NotFound(new ProblemDetails { Status = StatusCodes.Status404NotFound, Title = "El comentario no existe o fue eliminado." });
    }

    private async Task<ActionResult<EstadoMeGustaDto>> MarcarMeGusta(Guid id, bool meGusta, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var resultado = await comunidad.MarcarMeGustaAsync(usuarioId, id, meGusta, cancellationToken);
        return resultado is null
            ? NoEncontrada()
            : Ok(new EstadoMeGustaDto { MeGusta = resultado.MeGusta, TotalReacciones = resultado.TotalReacciones });
    }

    private RespuestaPublicacionFeedDto ARespuesta(PublicacionFeed resultado, Guid usuarioId)
        => MapeoComunidad.Publicacion(resultado, usuarioId, UrlArchivo, clave => Request.Firmada(clave, firmas));

    private string? UrlArchivo(string? clave) => Request.Publica(clave);
}
