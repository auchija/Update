using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de ciudades; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/ciudades")]
public sealed class CiudadesController(ServicioCrud<Ciudad> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaCiudadDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RespuestaCiudadDto>> Obtener(int id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaCiudadDto>> Crear([FromBody] CrearCiudadDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Ciudad
        {
            DepartamentoId = entrada.DepartamentoId,
            Nombre = entrada.Nombre,
            Codigo = entrada.Codigo,
            Latitud = entrada.Latitud,
            Longitud = entrada.Longitud
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarCiudadDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.DepartamentoId = entrada.DepartamentoId;
        entidad.Nombre = entrada.Nombre;
        entidad.Codigo = entrada.Codigo;
        entidad.Latitud = entrada.Latitud;
        entidad.Longitud = entrada.Longitud;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([id], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaCiudadDto ARespuesta(Ciudad entidad) => new()
    {
        Id = entidad.Id,
        DepartamentoId = entidad.DepartamentoId,
        Nombre = entidad.Nombre,
        Codigo = entidad.Codigo,
        Latitud = entidad.Latitud,
        Longitud = entidad.Longitud
    };
}
