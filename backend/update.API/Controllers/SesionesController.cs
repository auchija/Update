using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de sesiones; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/sesiones")]
public sealed class SesionesController(ServicioCrud<Sesion> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaSesionDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaSesionDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaSesionDto>> Crear([FromBody] CrearSesionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Sesion
        {
            UsuarioId = entrada.UsuarioId,
            HashTokenRefresco = entrada.HashTokenRefresco,
            FamiliaId = entrada.FamiliaId,
            ReemplazadoPorId = entrada.ReemplazadoPorId,
            AgenteUsuario = entrada.AgenteUsuario,
            DireccionIp = entrada.DireccionIp,
            ExpiraEn = entrada.ExpiraEn,
            RevocadoEn = entrada.RevocadoEn,
            MotivoRevocacion = entrada.MotivoRevocacion
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarSesionDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.UsuarioId = entrada.UsuarioId;
        entidad.HashTokenRefresco = entrada.HashTokenRefresco;
        entidad.FamiliaId = entrada.FamiliaId;
        entidad.ReemplazadoPorId = entrada.ReemplazadoPorId;
        entidad.AgenteUsuario = entrada.AgenteUsuario;
        entidad.DireccionIp = entrada.DireccionIp;
        entidad.ExpiraEn = entrada.ExpiraEn;
        entidad.RevocadoEn = entrada.RevocadoEn;
        entidad.MotivoRevocacion = entrada.MotivoRevocacion;
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

    private static RespuestaSesionDto ARespuesta(Sesion entidad) => new()
    {
        Id = entidad.Id,
        UsuarioId = entidad.UsuarioId,
        FamiliaId = entidad.FamiliaId,
        ReemplazadoPorId = entidad.ReemplazadoPorId,
        AgenteUsuario = entidad.AgenteUsuario,
        DireccionIp = entidad.DireccionIp,
        CreadoEn = entidad.CreadoEn,
        UltimoUsoEn = entidad.UltimoUsoEn,
        ExpiraEn = entidad.ExpiraEn,
        RevocadoEn = entidad.RevocadoEn,
        MotivoRevocacion = entidad.MotivoRevocacion
    };
}
