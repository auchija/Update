using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de reacciones_comentario; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/reacciones_comentario")]
public sealed class ReaccionesComentarioController(ServicioCrud<ReaccionComentario> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaReaccionComentarioDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{comentarioId:guid}/{perfilId:guid}")]
    public async Task<ActionResult<RespuestaReaccionComentarioDto>> Obtener(Guid comentarioId, Guid perfilId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([comentarioId, perfilId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaReaccionComentarioDto>> Crear([FromBody] CrearReaccionComentarioDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new ReaccionComentario
        {
            ComentarioId = entrada.ComentarioId,
            PerfilId = entrada.PerfilId,
            TipoReaccionCodigo = entrada.TipoReaccionCodigo
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { comentarioId = entidad.ComentarioId, perfilId = entidad.PerfilId }, ARespuesta(entidad));
    }

    [HttpPut("{comentarioId:guid}/{perfilId:guid}")]
    public async Task<IActionResult> Actualizar(Guid comentarioId, Guid perfilId, [FromBody] ActualizarReaccionComentarioDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([comentarioId, perfilId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.TipoReaccionCodigo = entrada.TipoReaccionCodigo;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{comentarioId:guid}/{perfilId:guid}")]
    public async Task<IActionResult> Eliminar(Guid comentarioId, Guid perfilId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([comentarioId, perfilId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaReaccionComentarioDto ARespuesta(ReaccionComentario entidad) => new()
    {
        ComentarioId = entidad.ComentarioId,
        PerfilId = entidad.PerfilId,
        TipoReaccionCodigo = entidad.TipoReaccionCodigo,
        CreadoEn = entidad.CreadoEn
    };
}
