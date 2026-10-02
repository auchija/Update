using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de cotizaciones; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/cotizaciones")]
public sealed class CotizacionesController(ServicioCrud<Cotizacion> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaCotizacionDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaCotizacionDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaCotizacionDto>> Crear([FromBody] CrearCotizacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Cotizacion
        {
            SolicitudCotizacionId = entrada.SolicitudCotizacionId,
            Version = entrada.Version,
            CreadoPorUsuarioId = entrada.CreadoPorUsuarioId,
            MonedaCodigo = entrada.MonedaCodigo,
            Subtotal = entrada.Subtotal,
            Descuento = entrada.Descuento,
            Impuesto = entrada.Impuesto,
            ValidoHasta = entrada.ValidoHasta,
            Notas = entrada.Notas,
            Estado = entrada.Estado
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarCotizacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.Version = entrada.Version;
        entidad.CreadoPorUsuarioId = entrada.CreadoPorUsuarioId;
        entidad.MonedaCodigo = entrada.MonedaCodigo;
        entidad.Subtotal = entrada.Subtotal;
        entidad.Descuento = entrada.Descuento;
        entidad.Impuesto = entrada.Impuesto;
        entidad.ValidoHasta = entrada.ValidoHasta;
        entidad.Notas = entrada.Notas;
        entidad.Estado = entrada.Estado;
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

    private static RespuestaCotizacionDto ARespuesta(Cotizacion entidad) => new()
    {
        Id = entidad.Id,
        SolicitudCotizacionId = entidad.SolicitudCotizacionId,
        Version = entidad.Version,
        CreadoPorUsuarioId = entidad.CreadoPorUsuarioId,
        MonedaCodigo = entidad.MonedaCodigo,
        Subtotal = entidad.Subtotal,
        Descuento = entidad.Descuento,
        Impuesto = entidad.Impuesto,
        Total = entidad.Total,
        ValidoHasta = entidad.ValidoHasta,
        Notas = entidad.Notas,
        Estado = entidad.Estado,
        CreadoEn = entidad.CreadoEn
    };
}
