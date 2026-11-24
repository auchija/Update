using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;

namespace update.API.Controllers;

/// <summary>Perfiles públicos, sus publicaciones, seguir perfiles y sugerencias para descubrir gente.</summary>
[ApiController]
[Authorize]
[Route("api/comunidad")]
public sealed class ComunidadController(ServicioComunidad servicio, FirmaArchivos firmas) : ControllerBase
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
    [EndpointSummary("Devuelve un perfil por su @usuario.")]
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
            : Ok(publicaciones.Select(p => MapeoComunidad.Publicacion(p, usuarioId, UrlArchivo, clave => Request.Firmada(clave, firmas))).ToList());
    }

    /// <summary>Quienes siguen al perfil. En una cuenta privada, solo para su dueño y sus seguidores.</summary>
    [HttpGet("perfiles/{nombreUsuario}/seguidores")]
    [ProducesResponseType<IReadOnlyList<PerfilPublicoDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public Task<ActionResult<IReadOnlyList<PerfilPublicoDto>>> Seguidores(
        string nombreUsuario,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 30,
        CancellationToken cancellationToken = default)
        => ListaDePerfiles((usuarioId) => servicio.ListarSeguidoresAsync(usuarioId, nombreUsuario, pagina, tamanoPagina, cancellationToken));

    /// <summary>A quiénes sigue el perfil. En una cuenta privada, solo para su dueño y sus seguidores.</summary>
    [HttpGet("perfiles/{nombreUsuario}/seguidos")]
    [ProducesResponseType<IReadOnlyList<PerfilPublicoDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public Task<ActionResult<IReadOnlyList<PerfilPublicoDto>>> Seguidos(
        string nombreUsuario,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 30,
        CancellationToken cancellationToken = default)
        => ListaDePerfiles((usuarioId) => servicio.ListarSeguidosAsync(usuarioId, nombreUsuario, pagina, tamanoPagina, cancellationToken));

    /// <summary>Perfiles relacionados: los que también siguen sus seguidores; se completa con los más activos.</summary>
    [HttpGet("perfiles/{nombreUsuario}/relacionados")]
    [ProducesResponseType<IReadOnlyList<PerfilPublicoDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public Task<ActionResult<IReadOnlyList<PerfilPublicoDto>>> Relacionados(
        string nombreUsuario,
        [FromQuery] int cantidad = 8,
        CancellationToken cancellationToken = default)
        => ListaDePerfiles((usuarioId) => servicio.RelacionadosAsync(usuarioId, nombreUsuario, cantidad, cancellationToken));

    /// <summary>Solicitudes pendientes para seguir mi cuenta, las más recientes primero.</summary>
    [HttpGet("solicitudes")]
    [ProducesResponseType<IReadOnlyList<SolicitudSeguimientoDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<SolicitudSeguimientoDto>>> Solicitudes(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 50,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var solicitudes = await servicio.ListarSolicitudesAsync(usuarioId, pagina, tamanoPagina, cancellationToken);
        return Ok(solicitudes.Select(s => new SolicitudSeguimientoDto
        {
            Perfil = MapeoComunidad.Perfil(s, UrlArchivo),
            CreadoEn = s.Fecha ?? DateTime.UtcNow
        }).ToList());
    }

    /// <summary>Acepta la solicitud de ese @usuario: pasa a ser seguidor.</summary>
    [HttpPost("solicitudes/{nombreUsuario}/aceptar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public Task<IActionResult> AceptarSolicitud(string nombreUsuario, CancellationToken cancellationToken)
        => ResponderSolicitud(nombreUsuario, true, cancellationToken);

    /// <summary>Rechaza la solicitud de ese @usuario.</summary>
    [HttpDelete("solicitudes/{nombreUsuario}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public Task<IActionResult> RechazarSolicitud(string nombreUsuario, CancellationToken cancellationToken)
        => ResponderSolicitud(nombreUsuario, false, cancellationToken);

    /// <summary>Sigue el perfil; si es privado, envía una solicitud. Repetirlo no suma otra vez.</summary>
    [HttpPost("perfiles/{nombreUsuario}/seguir")]
    [ProducesResponseType<EstadoSeguimientoDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public Task<ActionResult<EstadoSeguimientoDto>> Seguir(string nombreUsuario, CancellationToken cancellationToken)
        => CambiarSeguimiento(nombreUsuario, true, cancellationToken);

    /// <summary>Deja de seguir el perfil o cancela la solicitud. Repetirlo no resta otra vez.</summary>
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
            : Ok(new EstadoSeguimientoDto
            {
                LoSigo = resultado.LoSigo,
                SolicitudPendiente = resultado.SolicitudPendiente,
                TotalSeguidores = resultado.TotalSeguidores
            });
    }

    private async Task<IActionResult> ResponderSolicitud(string nombreUsuario, bool aceptar, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        return await servicio.ResponderSolicitudAsync(usuarioId, nombreUsuario, aceptar, cancellationToken)
            ? NoContent()
            : NotFound(new ProblemDetails { Status = StatusCodes.Status404NotFound, Title = "Esa solicitud ya no existe." });
    }

    private async Task<ActionResult<IReadOnlyList<PerfilPublicoDto>>> ListaDePerfiles(
        Func<Guid, Task<IReadOnlyList<PerfilVista>?>> consulta)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var perfiles = await consulta(usuarioId);
        return perfiles is null ? NoEncontrado() : Ok(perfiles.Select(p => MapeoComunidad.Perfil(p, UrlArchivo)).ToList());
    }

    private ObjectResult NoEncontrado() => NotFound(new ProblemDetails
    {
        Status = StatusCodes.Status404NotFound,
        Title = "Ese perfil no existe."
    });

    private string? UrlArchivo(string? clave) => Request.Publica(clave);
}
