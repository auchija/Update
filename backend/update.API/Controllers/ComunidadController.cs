using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;

namespace update.API.Controllers;

/// <summary>Perfiles públicos, sus publicaciones, seguir perfiles y sugerencias para descubrir gente.</summary>
[ApiController]
[Authorize]
[Route("api/comunidad")]
public sealed class ComunidadController(ServicioComunidad servicio) : ControllerBase
{
    /// <summary>Perfiles activos para descubrir (los que más publican), con la marca <c>loSigo</c>.</summary>
    [HttpGet("sugerencias")]
    [ProducesResponseType<IReadOnlyList<PerfilPublicoDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PerfilPublicoDto>>> Sugerencias(
        [FromQuery] int cantidad = 10,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var perfiles = await servicio.SugerenciasAsync(usuarioId, cantidad, cancellationToken);
        return Ok(perfiles.Select(p => MapeoComunidad.Perfil(p, UrlArchivo)).ToList());
    }

    [HttpGet("perfiles/{nombreUsuario}")]
    [ProducesResponseType<PerfilPublicoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PerfilPublicoDto>> Perfil(string nombreUsuario, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var perfil = await servicio.ObtenerPerfilAsync(usuarioId, nombreUsuario, cancellationToken);
        return perfil is null ? NoEncontrado() : Ok(MapeoComunidad.Perfil(perfil, UrlArchivo));
    }

    /// <summary>Publicaciones públicas del perfil, de la más nueva a la más antigua.</summary>
    [HttpGet("perfiles/{nombreUsuario}/publicaciones")]
    [ProducesResponseType<IReadOnlyList<RespuestaPublicacionFeedDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<RespuestaPublicacionFeedDto>>> Publicaciones(
        string nombreUsuario,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 20,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var publicaciones = await servicio.ListarPublicacionesDePerfilAsync(usuarioId, nombreUsuario, pagina, tamanoPagina, cancellationToken);
        return publicaciones is null
            ? NoEncontrado()
            : Ok(publicaciones.Select(p => MapeoComunidad.Publicacion(p, usuarioId, UrlArchivo)).ToList());
    }

    /// <summary>Sigue el perfil. Repetirlo no suma otra vez.</summary>
    [HttpPost("perfiles/{nombreUsuario}/seguir")]
    [ProducesResponseType<EstadoSeguimientoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public Task<ActionResult<EstadoSeguimientoDto>> Seguir(string nombreUsuario, CancellationToken cancellationToken)
        => CambiarSeguimiento(nombreUsuario, true, cancellationToken);

    /// <summary>Deja de seguir el perfil. Repetirlo no resta otra vez.</summary>
    [HttpDelete("perfiles/{nombreUsuario}/seguir")]
    [ProducesResponseType<EstadoSeguimientoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public Task<ActionResult<EstadoSeguimientoDto>> DejarDeSeguir(string nombreUsuario, CancellationToken cancellationToken)
        => CambiarSeguimiento(nombreUsuario, false, cancellationToken);

    private async Task<ActionResult<EstadoSeguimientoDto>> CambiarSeguimiento(string nombreUsuario, bool seguir, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var resultado = await servicio.SeguirAsync(usuarioId, nombreUsuario, seguir, cancellationToken);
        return resultado is null
            ? NoEncontrado()
            : Ok(new EstadoSeguimientoDto { LoSigo = resultado.LoSigo, TotalSeguidores = resultado.TotalSeguidores });
    }

    private ObjectResult NoEncontrado() => NotFound(new ProblemDetails
    {
        Status = StatusCodes.Status404NotFound,
        Title = "Ese perfil no existe."
    });

    private string? UrlArchivo(string? clave) => clave is null ? null : $"{Request.Scheme}://{Request.Host}/uploads/{clave}";
}
