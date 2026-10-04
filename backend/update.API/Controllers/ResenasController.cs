using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de resenas; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/resenas")]
public sealed class ResenasController(ServicioCrud<Resena> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaResenaDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaResenaDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaResenaDto>> Crear([FromBody] CrearResenaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Resena
        {
            UsuarioResenadorId = entrada.UsuarioResenadorId,
            EmprendimientoId = entrada.EmprendimientoId,
            ProductoId = entrada.ProductoId,
            SolicitudCotizacionId = entrada.SolicitudCotizacionId,
            Calificacion = entrada.Calificacion,
            Titulo = entrada.Titulo,
            Contenido = entrada.Contenido,
            Estado = entrada.Estado,
            RespuestaEmprendimiento = entrada.RespuestaEmprendimiento,
            RespondidoPorUsuarioId = entrada.RespondidoPorUsuarioId,
            RespondidoEn = entrada.RespondidoEn
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarResenaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.UsuarioResenadorId = entrada.UsuarioResenadorId;
        entidad.EmprendimientoId = entrada.EmprendimientoId;
        entidad.ProductoId = entrada.ProductoId;
        entidad.SolicitudCotizacionId = entrada.SolicitudCotizacionId;
        entidad.Calificacion = entrada.Calificacion;
        entidad.Titulo = entrada.Titulo;
        entidad.Contenido = entrada.Contenido;
        entidad.Estado = entrada.Estado;
        entidad.RespuestaEmprendimiento = entrada.RespuestaEmprendimiento;
        entidad.RespondidoPorUsuarioId = entrada.RespondidoPorUsuarioId;
        entidad.RespondidoEn = entrada.RespondidoEn;
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

    private static RespuestaResenaDto ARespuesta(Resena entidad) => new()
    {
        Id = entidad.Id,
        UsuarioResenadorId = entidad.UsuarioResenadorId,
        EmprendimientoId = entidad.EmprendimientoId,
        ProductoId = entidad.ProductoId,
        SolicitudCotizacionId = entidad.SolicitudCotizacionId,
        Calificacion = entidad.Calificacion,
        Titulo = entidad.Titulo,
        Contenido = entidad.Contenido,
        Estado = entidad.Estado,
        RespuestaEmprendimiento = entidad.RespuestaEmprendimiento,
        RespondidoPorUsuarioId = entidad.RespondidoPorUsuarioId,
        RespondidoEn = entidad.RespondidoEn,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn,
        EliminadoEn = entidad.EliminadoEn
    };
}
