using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de usuarios_reservados; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/usuarios_reservados")]
public sealed class UsuariosReservadosController(ServicioCrud<UsuarioReservado> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaUsuarioReservadoDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{nombreUsuario}")]
    public async Task<ActionResult<RespuestaUsuarioReservadoDto>> Obtener(string nombreUsuario, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([nombreUsuario], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaUsuarioReservadoDto>> Crear([FromBody] CrearUsuarioReservadoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new UsuarioReservado
        {
            NombreUsuario = entrada.NombreUsuario
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { nombreUsuario = entidad.NombreUsuario }, ARespuesta(entidad));
    }

    [HttpPut("{nombreUsuario}")]
    public async Task<IActionResult> Actualizar(string nombreUsuario, [FromBody] ActualizarUsuarioReservadoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([nombreUsuario], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });

        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{nombreUsuario}")]
    public async Task<IActionResult> Eliminar(string nombreUsuario, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([nombreUsuario], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaUsuarioReservadoDto ARespuesta(UsuarioReservado entidad) => new()
    {
        NombreUsuario = entidad.NombreUsuario
    };
}
