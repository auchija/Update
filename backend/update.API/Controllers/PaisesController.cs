using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de paises; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/paises")]
public sealed class PaisesController(ServicioCrud<Pais> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaPaisDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{codigo}")]
    public async Task<ActionResult<RespuestaPaisDto>> Obtener(string codigo, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([codigo], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaPaisDto>> Crear([FromBody] CrearPaisDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Pais
        {
            Codigo = entrada.Codigo,
            Nombre = entrada.Nombre,
            MonedaDefectoCodigo = entrada.MonedaDefectoCodigo,
            PrefijoTelefono = entrada.PrefijoTelefono
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { codigo = entidad.Codigo }, ARespuesta(entidad));
    }

    [HttpPut("{codigo}")]
    public async Task<IActionResult> Actualizar(string codigo, [FromBody] ActualizarPaisDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([codigo], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.Nombre = entrada.Nombre;
        entidad.MonedaDefectoCodigo = entrada.MonedaDefectoCodigo;
        entidad.PrefijoTelefono = entrada.PrefijoTelefono;
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

    private static RespuestaPaisDto ARespuesta(Pais entidad) => new()
    {
        Codigo = entidad.Codigo,
        Nombre = entidad.Nombre,
        MonedaDefectoCodigo = entidad.MonedaDefectoCodigo,
        PrefijoTelefono = entidad.PrefijoTelefono
    };
}
