using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de intereses_usuario; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/intereses_usuario")]
public sealed class InteresesUsuarioController(ServicioCrud<InteresUsuario> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaInteresUsuarioDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{usuarioId:guid}/{categoriaId:int}")]
    public async Task<ActionResult<RespuestaInteresUsuarioDto>> Obtener(Guid usuarioId, int categoriaId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([usuarioId, categoriaId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaInteresUsuarioDto>> Crear([FromBody] CrearInteresUsuarioDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new InteresUsuario
        {
            UsuarioId = entrada.UsuarioId,
            CategoriaId = entrada.CategoriaId
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { usuarioId = entidad.UsuarioId, categoriaId = entidad.CategoriaId }, ARespuesta(entidad));
    }

    [HttpPut("{usuarioId:guid}/{categoriaId:int}")]
    public async Task<IActionResult> Actualizar(Guid usuarioId, int categoriaId, [FromBody] ActualizarInteresUsuarioDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([usuarioId, categoriaId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });

        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{usuarioId:guid}/{categoriaId:int}")]
    public async Task<IActionResult> Eliminar(Guid usuarioId, int categoriaId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([usuarioId, categoriaId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaInteresUsuarioDto ARespuesta(InteresUsuario entidad) => new()
    {
        UsuarioId = entidad.UsuarioId,
        CategoriaId = entidad.CategoriaId,
        CreadoEn = entidad.CreadoEn
    };
}
