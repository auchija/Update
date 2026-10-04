using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de tipos_publicacion; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/tipos_publicacion")]
public sealed class TiposPublicacionController(ServicioCrud<TipoPublicacion> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaTipoPublicacionDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{codigo}")]
    public async Task<ActionResult<RespuestaTipoPublicacionDto>> Obtener(string codigo, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([codigo], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaTipoPublicacionDto>> Crear([FromBody] CrearTipoPublicacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new TipoPublicacion
        {
            Codigo = entrada.Codigo,
            Nombre = entrada.Nombre,
            Descripcion = entrada.Descripcion,
            Icono = entrada.Icono,
            PesoDescubrimiento = entrada.PesoDescubrimiento,
            PermiteRespuestaAceptada = entrada.PermiteRespuestaAceptada,
            Orden = entrada.Orden,
            Activo = entrada.Activo
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { codigo = entidad.Codigo }, ARespuesta(entidad));
    }

    [HttpPut("{codigo}")]
    public async Task<IActionResult> Actualizar(string codigo, [FromBody] ActualizarTipoPublicacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([codigo], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.Nombre = entrada.Nombre;
        entidad.Descripcion = entrada.Descripcion;
        entidad.Icono = entrada.Icono;
        entidad.PesoDescubrimiento = entrada.PesoDescubrimiento;
        entidad.PermiteRespuestaAceptada = entrada.PermiteRespuestaAceptada;
        entidad.Orden = entrada.Orden;
        entidad.Activo = entrada.Activo;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{codigo}")]
    public async Task<IActionResult> Eliminar(string codigo, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([codigo], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaTipoPublicacionDto ARespuesta(TipoPublicacion entidad) => new()
    {
        Codigo = entidad.Codigo,
        Nombre = entidad.Nombre,
        Descripcion = entidad.Descripcion,
        Icono = entidad.Icono,
        PesoDescubrimiento = entidad.PesoDescubrimiento,
        PermiteRespuestaAceptada = entidad.PermiteRespuestaAceptada,
        Orden = entidad.Orden,
        Activo = entidad.Activo
    };
}
