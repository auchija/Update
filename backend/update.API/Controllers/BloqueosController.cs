using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de bloqueos; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/bloqueos")]
public sealed class BloqueosController(ServicioCrud<Bloqueo> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaBloqueoDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{usuarioBloqueadorId:guid}/{perfilBloqueadoId:guid}")]
    public async Task<ActionResult<RespuestaBloqueoDto>> Obtener(Guid usuarioBloqueadorId, Guid perfilBloqueadoId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([usuarioBloqueadorId, perfilBloqueadoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaBloqueoDto>> Crear([FromBody] CrearBloqueoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Bloqueo
        {
            UsuarioBloqueadorId = entrada.UsuarioBloqueadorId,
            PerfilBloqueadoId = entrada.PerfilBloqueadoId
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { usuarioBloqueadorId = entidad.UsuarioBloqueadorId, perfilBloqueadoId = entidad.PerfilBloqueadoId }, ARespuesta(entidad));
    }

    [HttpPut("{usuarioBloqueadorId:guid}/{perfilBloqueadoId:guid}")]
    public async Task<IActionResult> Actualizar(Guid usuarioBloqueadorId, Guid perfilBloqueadoId, [FromBody] ActualizarBloqueoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([usuarioBloqueadorId, perfilBloqueadoId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });

        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{usuarioBloqueadorId:guid}/{perfilBloqueadoId:guid}")]
    public async Task<IActionResult> Eliminar(Guid usuarioBloqueadorId, Guid perfilBloqueadoId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([usuarioBloqueadorId, perfilBloqueadoId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaBloqueoDto ARespuesta(Bloqueo entidad) => new()
    {
        UsuarioBloqueadorId = entidad.UsuarioBloqueadorId,
        PerfilBloqueadoId = entidad.PerfilBloqueadoId,
        CreadoEn = entidad.CreadoEn
    };
}
