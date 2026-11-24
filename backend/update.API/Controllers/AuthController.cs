using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using update.Application.DTOs;
using update.Application.Services;

namespace update.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("register")]
    [EndpointSummary("Crea la cuenta y su perfil, y abre la sesión.")]
    [EnableRateLimiting("autenticacion")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UsuarioSesionDto))]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registrar(
        [FromBody] RegistroRequest request,
        CancellationToken cancellationToken)
    {
        var respuesta = await authService.RegistrarAsync(request, cancellationToken);

        Response.Cookies.Append("refreshToken", respuesta.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/api/auth",
            Expires = respuesta.RefreshTokenExpiresAt
        });

        return StatusCode(StatusCodes.Status201Created, new
        {
            accessToken = respuesta.AccessToken,
            user = ConAvatarAbsoluto(respuesta.User)
        });
    }

    [HttpPost("login")]
    [EndpointSummary("Inicia sesión con correo o @usuario; bloquea 15 minutos tras 5 intentos fallidos.")]
    [EnableRateLimiting("autenticacion")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioSesionDto))]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> IniciarSesion(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var respuesta = await authService.IniciarSesionAsync(
            request, cancellationToken);

        if (respuesta is null)
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Los datos no coinciden. Revisa tu correo o usuario y tu contraseña."
            });
        }

        Response.Cookies.Append("refreshToken", respuesta.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/api/auth",
            Expires = respuesta.RefreshTokenExpiresAt
        });

        return Ok(new
        {
            accessToken = respuesta.AccessToken,
            user = ConAvatarAbsoluto(respuesta.User)
        });
    }

    [HttpPost("refresh")]
    [EndpointSummary("Renueva el token de acceso con la cookie de sesión.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioSesionDto))]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            return Unauthorized();

        var respuesta = await authService.RefreshAsync(refreshToken, cancellationToken);

        if (respuesta is null)
            return Unauthorized();

        Response.Cookies.Append("refreshToken", respuesta.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/api/auth",
            Expires = respuesta.RefreshTokenExpiresAt
        });

        return Ok(new
        {
            accessToken = respuesta.AccessToken,
            user = ConAvatarAbsoluto(respuesta.User)
        });
    }

    [HttpPost("logout")]
    [EndpointSummary("Cierra la sesión y anula la cookie.")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            await authService.LogoutAsync(refreshToken, cancellationToken);

        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/api/auth"
        });

        return NoContent();
    }

    [HttpGet("me")]
    [EndpointSummary("Devuelve el usuario con sesión.")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioSesionDto))]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value;

        if (userId is null)
            return Unauthorized();

        var user = await authService.ObtenerUsuarioAsync(userId, cancellationToken);

        return user is null ? NotFound() : Ok(ConAvatarAbsoluto(user));
    }

    private UsuarioSesionDto ConAvatarAbsoluto(UsuarioSesionDto user) => user.AvatarUrl is null
        ? user
        : user with { AvatarUrl = $"{Request.Scheme}://{Request.Host}{user.AvatarUrl}" };
}
