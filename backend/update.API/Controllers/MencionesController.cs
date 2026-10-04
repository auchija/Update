using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de menciones; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/menciones")]
public sealed class MencionesController(ServicioCrud<Mencion> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaMencionDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{publicacionId:guid}/{perfilId:guid}")]
    public async Task<ActionResult<RespuestaMencionDto>> Obtener(Guid publicacionId, Guid perfilId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([publicacionId, perfilId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaMencionDto>> Crear([FromBody] CrearMencionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Mencion
        {
            PublicacionId = entrada.PublicacionId,
            PerfilId = entrada.PerfilId
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { publicacionId = entidad.PublicacionId, perfilId = entidad.PerfilId }, ARespuesta(entidad));
    }

    [HttpPut("{publicacionId:guid}/{perfilId:guid}")]
    public async Task<IActionResult> Actualizar(Guid publicacionId, Guid perfilId, [FromBody] ActualizarMencionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([publicacionId, perfilId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });

        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{publicacionId:guid}/{perfilId:guid}")]
    public async Task<IActionResult> Eliminar(Guid publicacionId, Guid perfilId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([publicacionId, perfilId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaMencionDto ARespuesta(Mencion entidad) => new()
    {
        PublicacionId = entidad.PublicacionId,
        PerfilId = entidad.PerfilId
    };
}
