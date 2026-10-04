using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de comentarios; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/comentarios")]
public sealed class ComentariosController(ServicioCrud<Comentario> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaComentarioDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaComentarioDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaComentarioDto>> Crear([FromBody] CrearComentarioDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Comentario
        {
            PublicacionId = entrada.PublicacionId,
            ComentarioPadreId = entrada.ComentarioPadreId,
            PerfilAutorId = entrada.PerfilAutorId,
            CreadoPorUsuarioId = entrada.CreadoPorUsuarioId,
            RespondeAPerfilId = entrada.RespondeAPerfilId,
            Contenido = entrada.Contenido,
            TotalReacciones = entrada.TotalReacciones,
            TotalRespuestas = entrada.TotalRespuestas
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarComentarioDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.ComentarioPadreId = entrada.ComentarioPadreId;
        entidad.PerfilAutorId = entrada.PerfilAutorId;
        entidad.CreadoPorUsuarioId = entrada.CreadoPorUsuarioId;
        entidad.RespondeAPerfilId = entrada.RespondeAPerfilId;
        entidad.Contenido = entrada.Contenido;
        entidad.TotalReacciones = entrada.TotalReacciones;
        entidad.TotalRespuestas = entrada.TotalRespuestas;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([id], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaComentarioDto ARespuesta(Comentario entidad) => new()
    {
        Id = entidad.Id,
        PublicacionId = entidad.PublicacionId,
        ComentarioPadreId = entidad.ComentarioPadreId,
        PerfilAutorId = entidad.PerfilAutorId,
        CreadoPorUsuarioId = entidad.CreadoPorUsuarioId,
        RespondeAPerfilId = entidad.RespondeAPerfilId,
        Contenido = entidad.Contenido,
        TotalReacciones = entidad.TotalReacciones,
        TotalRespuestas = entidad.TotalRespuestas,
        EditadoEn = entidad.EditadoEn,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn,
        EliminadoEn = entidad.EliminadoEn
    };
}
