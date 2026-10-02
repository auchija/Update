using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de publicaciones; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/publicaciones")]
public sealed class PublicacionesController(ServicioCrud<Publicacion> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaPublicacionDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaPublicacionDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaPublicacionDto>> Crear([FromBody] CrearPublicacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Publicacion
        {
            PerfilAutorId = entrada.PerfilAutorId,
            CreadoPorUsuarioId = entrada.CreadoPorUsuarioId,
            TipoPublicacionCodigo = entrada.TipoPublicacionCodigo,
            Contenido = entrada.Contenido,
            Visibilidad = entrada.Visibilidad,
            ProductoId = entrada.ProductoId,
            OportunidadId = entrada.OportunidadId,
            PublicacionCompartidaId = entrada.PublicacionCompartidaId,
            ComentarioAceptadoId = entrada.ComentarioAceptadoId,
            ComentariosHabilitados = entrada.ComentariosHabilitados,
            Fijado = entrada.Fijado,
            TotalReacciones = entrada.TotalReacciones,
            TotalComentarios = entrada.TotalComentarios,
            TotalCompartidos = entrada.TotalCompartidos,
            TotalGuardados = entrada.TotalGuardados,
            PuntajeDescubrimiento = entrada.PuntajeDescubrimiento,
            PuntajeActualizadoEn = entrada.PuntajeActualizadoEn
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarPublicacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.PerfilAutorId = entrada.PerfilAutorId;
        entidad.CreadoPorUsuarioId = entrada.CreadoPorUsuarioId;
        entidad.TipoPublicacionCodigo = entrada.TipoPublicacionCodigo;
        entidad.Contenido = entrada.Contenido;
        entidad.Visibilidad = entrada.Visibilidad;
        entidad.ProductoId = entrada.ProductoId;
        entidad.OportunidadId = entrada.OportunidadId;
        entidad.PublicacionCompartidaId = entrada.PublicacionCompartidaId;
        entidad.ComentarioAceptadoId = entrada.ComentarioAceptadoId;
        entidad.ComentariosHabilitados = entrada.ComentariosHabilitados;
        entidad.Fijado = entrada.Fijado;
        entidad.TotalReacciones = entrada.TotalReacciones;
        entidad.TotalComentarios = entrada.TotalComentarios;
        entidad.TotalCompartidos = entrada.TotalCompartidos;
        entidad.TotalGuardados = entrada.TotalGuardados;
        entidad.PuntajeDescubrimiento = entrada.PuntajeDescubrimiento;
        entidad.PuntajeActualizadoEn = entrada.PuntajeActualizadoEn;
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

    private static RespuestaPublicacionDto ARespuesta(Publicacion entidad) => new()
    {
        Id = entidad.Id,
        PerfilAutorId = entidad.PerfilAutorId,
        CreadoPorUsuarioId = entidad.CreadoPorUsuarioId,
        TipoPublicacionCodigo = entidad.TipoPublicacionCodigo,
        Contenido = entidad.Contenido,
        Visibilidad = entidad.Visibilidad,
        ProductoId = entidad.ProductoId,
        OportunidadId = entidad.OportunidadId,
        PublicacionCompartidaId = entidad.PublicacionCompartidaId,
        ComentarioAceptadoId = entidad.ComentarioAceptadoId,
        ComentariosHabilitados = entidad.ComentariosHabilitados,
        Fijado = entidad.Fijado,
        TotalReacciones = entidad.TotalReacciones,
        TotalComentarios = entidad.TotalComentarios,
        TotalCompartidos = entidad.TotalCompartidos,
        TotalGuardados = entidad.TotalGuardados,
        PuntajeDescubrimiento = entidad.PuntajeDescubrimiento,
        PuntajeActualizadoEn = entidad.PuntajeActualizadoEn,
        EditadoEn = entidad.EditadoEn,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn,
        EliminadoEn = entidad.EliminadoEn
    };
}
