using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;

namespace update.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Registrar(
        [FromBody] RegistroRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var respuesta = await authService.RegistrarAsync(
    request, cancellationToken);

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
                user = respuesta.User
            });
        }
        catch (InvalidOperationException)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "No se pudo crear la cuenta."
            });
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
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
                Title = "Correo o contraseña incorrectos."
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
            user = respuesta.User
        });
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
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
            user = respuesta.User
        });
    }

    [HttpPost("logout")]
    [AllowAnonymous]
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
    [Authorize]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
            return Unauthorized();

        var user = await authService.ObtenerUsuarioAsync(userId, cancellationToken);

        return user is null ? NotFound() : Ok(user);
    }
}
