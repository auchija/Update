using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de solicitudes_cotizacion; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/solicitudes_cotizacion")]
public sealed class SolicitudesCotizacionController(ServicioCrud<SolicitudCotizacion> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaSolicitudCotizacionDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaSolicitudCotizacionDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaSolicitudCotizacionDto>> Crear([FromBody] CrearSolicitudCotizacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new SolicitudCotizacion
        {
            EmprendimientoId = entrada.EmprendimientoId,
            PerfilSolicitanteId = entrada.PerfilSolicitanteId,
            CreadoPorUsuarioId = entrada.CreadoPorUsuarioId,
            ConversacionId = entrada.ConversacionId,
            Titulo = entrada.Titulo,
            Mensaje = entrada.Mensaje,
            FechaDeseada = entrada.FechaDeseada,
            CiudadEntregaId = entrada.CiudadEntregaId,
            Estado = entrada.Estado,
            CotizacionAceptadaId = entrada.CotizacionAceptadaId,
            CerradoEn = entrada.CerradoEn
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarSolicitudCotizacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.EmprendimientoId = entrada.EmprendimientoId;
        entidad.PerfilSolicitanteId = entrada.PerfilSolicitanteId;
        entidad.CreadoPorUsuarioId = entrada.CreadoPorUsuarioId;
        entidad.ConversacionId = entrada.ConversacionId;
        entidad.Titulo = entrada.Titulo;
        entidad.Mensaje = entrada.Mensaje;
        entidad.FechaDeseada = entrada.FechaDeseada;
        entidad.CiudadEntregaId = entrada.CiudadEntregaId;
        entidad.Estado = entrada.Estado;
        entidad.CotizacionAceptadaId = entrada.CotizacionAceptadaId;
        entidad.CerradoEn = entrada.CerradoEn;
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

    private static RespuestaSolicitudCotizacionDto ARespuesta(SolicitudCotizacion entidad) => new()
    {
        Id = entidad.Id,
        Numero = entidad.Numero,
        EmprendimientoId = entidad.EmprendimientoId,
        PerfilSolicitanteId = entidad.PerfilSolicitanteId,
        CreadoPorUsuarioId = entidad.CreadoPorUsuarioId,
        ConversacionId = entidad.ConversacionId,
        Titulo = entidad.Titulo,
        Mensaje = entidad.Mensaje,
        FechaDeseada = entidad.FechaDeseada,
        CiudadEntregaId = entidad.CiudadEntregaId,
        Estado = entidad.Estado,
        CotizacionAceptadaId = entidad.CotizacionAceptadaId,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn,
        CerradoEn = entidad.CerradoEn
    };
}
