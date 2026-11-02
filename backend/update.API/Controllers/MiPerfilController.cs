using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;

namespace update.API.Controllers;

[ApiController]
[Authorize]
[Route("api/perfil/yo")]
public sealed class MiPerfilController(ServicioMiPerfil servicio) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<RespuestaMiPerfilDto>(StatusCodes.Status200OK)]
    public async Task<ActionResult<RespuestaMiPerfilDto>> Obtener(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        var resultado = await servicio.ObtenerAsync(usuarioId, cancellationToken);
        return resultado is null ? NotFound() : Ok(ARespuesta(resultado));
    }

    [HttpPut]
    [ProducesResponseType<RespuestaMiPerfilDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RespuestaMiPerfilDto>> Actualizar(
        [FromBody] ActualizarMiPerfilDto entrada,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirst("sub")?.Value, out var usuarioId)) return Unauthorized();

        try
        {
            var resultado = await servicio.ActualizarAsync(usuarioId, entrada, cancellationToken);
            return resultado is null ? NotFound() : Ok(ARespuesta(resultado));
        }
        catch (NombreUsuarioNoDisponibleException ex)
        {
            return Conflict(new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = ex.Message,
                Extensions =
                {
                    ["message"] = "Revisa los datos marcados.",
                    ["errors"] = new Dictionary<string, string> { ["nombreUsuario"] = ex.Message }
                }
            });
        }
    }

    private RespuestaMiPerfilDto ARespuesta(MiPerfilResultado resultado) => new()
    {
        Id = resultado.Perfil.Id,
        NombreUsuario = resultado.Perfil.NombreUsuario,
        NombreVisible = resultado.Perfil.NombreVisible,
        Titular = resultado.Perfil.Titular,
        Biografia = resultado.Perfil.Biografia,
        SitioWeb = resultado.Perfil.SitioWeb,
        FotoArchivoId = resultado.Perfil.FotoArchivoId,
        FotoUrl = resultado.ClaveFoto is null ? null : $"{Request.Scheme}://{Request.Host}/uploads/{resultado.ClaveFoto}",
        TotalSeguidores = resultado.Perfil.TotalSeguidores,
        TotalSeguidos = resultado.Perfil.TotalSeguidos,
        TotalPublicaciones = resultado.Perfil.TotalPublicaciones
    };
}
