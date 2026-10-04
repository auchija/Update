using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de tokens_recuperacion; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/tokens_recuperacion")]
public sealed class TokensRecuperacionController(ServicioCrud<TokenRecuperacion> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaTokenRecuperacionDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaTokenRecuperacionDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaTokenRecuperacionDto>> Crear([FromBody] CrearTokenRecuperacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new TokenRecuperacion
        {
            UsuarioId = entrada.UsuarioId,
            HashToken = entrada.HashToken,
            IpSolicitud = entrada.IpSolicitud,
            ExpiraEn = entrada.ExpiraEn,
            UsadoEn = entrada.UsadoEn
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarTokenRecuperacionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.UsuarioId = entrada.UsuarioId;
        entidad.HashToken = entrada.HashToken;
        entidad.IpSolicitud = entrada.IpSolicitud;
        entidad.ExpiraEn = entrada.ExpiraEn;
        entidad.UsadoEn = entrada.UsadoEn;
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

    private static RespuestaTokenRecuperacionDto ARespuesta(TokenRecuperacion entidad) => new()
    {
        Id = entidad.Id,
        UsuarioId = entidad.UsuarioId,
        IpSolicitud = entidad.IpSolicitud,
        CreadoEn = entidad.CreadoEn,
        ExpiraEn = entidad.ExpiraEn,
        UsadoEn = entidad.UsadoEn
    };
}
