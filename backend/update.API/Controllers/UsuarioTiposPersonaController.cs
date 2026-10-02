using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de usuario_tipos_persona; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/usuario_tipos_persona")]
public sealed class UsuarioTiposPersonaController(ServicioCrud<UsuarioTipoPersona> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaUsuarioTipoPersonaDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{usuarioId:guid}/{tipoPersonaCodigo}")]
    public async Task<ActionResult<RespuestaUsuarioTipoPersonaDto>> Obtener(Guid usuarioId, string tipoPersonaCodigo, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([usuarioId, tipoPersonaCodigo], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaUsuarioTipoPersonaDto>> Crear([FromBody] CrearUsuarioTipoPersonaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new UsuarioTipoPersona
        {
            UsuarioId = entrada.UsuarioId,
            TipoPersonaCodigo = entrada.TipoPersonaCodigo
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { usuarioId = entidad.UsuarioId, tipoPersonaCodigo = entidad.TipoPersonaCodigo }, ARespuesta(entidad));
    }

    [HttpPut("{usuarioId:guid}/{tipoPersonaCodigo}")]
    public async Task<IActionResult> Actualizar(Guid usuarioId, string tipoPersonaCodigo, [FromBody] ActualizarUsuarioTipoPersonaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([usuarioId, tipoPersonaCodigo], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });

        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{usuarioId:guid}/{tipoPersonaCodigo}")]
    public async Task<IActionResult> Eliminar(Guid usuarioId, string tipoPersonaCodigo, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([usuarioId, tipoPersonaCodigo], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaUsuarioTipoPersonaDto ARespuesta(UsuarioTipoPersona entidad) => new()
    {
        UsuarioId = entidad.UsuarioId,
        TipoPersonaCodigo = entidad.TipoPersonaCodigo,
        CreadoEn = entidad.CreadoEn
    };
}
