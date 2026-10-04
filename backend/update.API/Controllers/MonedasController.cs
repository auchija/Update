using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de monedas; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/monedas")]
public sealed class MonedasController(ServicioCrud<Moneda> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaMonedaDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{codigo}")]
    public async Task<ActionResult<RespuestaMonedaDto>> Obtener(string codigo, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([codigo], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaMonedaDto>> Crear([FromBody] CrearMonedaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Moneda
        {
            Codigo = entrada.Codigo,
            Nombre = entrada.Nombre,
            Simbolo = entrada.Simbolo,
            Decimales = entrada.Decimales,
            Activo = entrada.Activo
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { codigo = entidad.Codigo }, ARespuesta(entidad));
    }

    [HttpPut("{codigo}")]
    public async Task<IActionResult> Actualizar(string codigo, [FromBody] ActualizarMonedaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([codigo], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.Nombre = entrada.Nombre;
        entidad.Simbolo = entrada.Simbolo;
        entidad.Decimales = entrada.Decimales;
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

    private static RespuestaMonedaDto ARespuesta(Moneda entidad) => new()
    {
        Codigo = entidad.Codigo,
        Nombre = entidad.Nombre,
        Simbolo = entidad.Simbolo,
        Decimales = entidad.Decimales,
        Activo = entidad.Activo
    };
}
