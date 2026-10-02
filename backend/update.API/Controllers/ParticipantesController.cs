using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de participantes; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/participantes")]
public sealed class ParticipantesController(ServicioCrud<Participante> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaParticipanteDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{conversacionId:guid}/{perfilId:guid}")]
    public async Task<ActionResult<RespuestaParticipanteDto>> Obtener(Guid conversacionId, Guid perfilId, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([conversacionId, perfilId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaParticipanteDto>> Crear([FromBody] CrearParticipanteDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Participante
        {
            ConversacionId = entrada.ConversacionId,
            PerfilId = entrada.PerfilId,
            Rol = entrada.Rol,
            UsuarioAsignadoId = entrada.UsuarioAsignadoId,
            SalioEn = entrada.SalioEn,
            UltimoMensajeLeidoId = entrada.UltimoMensajeLeidoId,
            UltimaLecturaEn = entrada.UltimaLecturaEn,
            SilenciadoHasta = entrada.SilenciadoHasta,
            ArchivadoEn = entrada.ArchivadoEn
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { conversacionId = entidad.ConversacionId, perfilId = entidad.PerfilId }, ARespuesta(entidad));
    }

    [HttpPut("{conversacionId:guid}/{perfilId:guid}")]
    public async Task<IActionResult> Actualizar(Guid conversacionId, Guid perfilId, [FromBody] ActualizarParticipanteDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([conversacionId, perfilId], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.Rol = entrada.Rol;
        entidad.UsuarioAsignadoId = entrada.UsuarioAsignadoId;
        entidad.SalioEn = entrada.SalioEn;
        entidad.UltimoMensajeLeidoId = entrada.UltimoMensajeLeidoId;
        entidad.UltimaLecturaEn = entrada.UltimaLecturaEn;
        entidad.SilenciadoHasta = entrada.SilenciadoHasta;
        entidad.ArchivadoEn = entrada.ArchivadoEn;
        await servicio.ActualizarAsync(entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{conversacionId:guid}/{perfilId:guid}")]
    public async Task<IActionResult> Eliminar(Guid conversacionId, Guid perfilId, CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync([conversacionId, perfilId], cancellationToken))
            return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return NoContent();
    }

    private static RespuestaParticipanteDto ARespuesta(Participante entidad) => new()
    {
        ConversacionId = entidad.ConversacionId,
        PerfilId = entidad.PerfilId,
        Rol = entidad.Rol,
        UsuarioAsignadoId = entidad.UsuarioAsignadoId,
        UnidoEn = entidad.UnidoEn,
        SalioEn = entidad.SalioEn,
        UltimoMensajeLeidoId = entidad.UltimoMensajeLeidoId,
        UltimaLecturaEn = entidad.UltimaLecturaEn,
        SilenciadoHasta = entidad.SilenciadoHasta,
        ArchivadoEn = entidad.ArchivadoEn
    };
}
