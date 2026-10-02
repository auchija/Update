using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de oportunidades; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/oportunidades")]
public sealed class OportunidadesController(ServicioCrud<Oportunidad> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaOportunidadDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaOportunidadDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaOportunidadDto>> Crear([FromBody] CrearOportunidadDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Oportunidad
        {
            PerfilAutorId = entrada.PerfilAutorId,
            CreadoPorUsuarioId = entrada.CreadoPorUsuarioId,
            TipoCodigo = entrada.TipoCodigo,
            Titulo = entrada.Titulo,
            Descripcion = entrada.Descripcion,
            CategoriaId = entrada.CategoriaId,
            CiudadId = entrada.CiudadId,
            EsRemoto = entrada.EsRemoto,
            Estado = entrada.Estado,
            ExpiraEn = entrada.ExpiraEn,
            TotalPostulaciones = entrada.TotalPostulaciones
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarOportunidadDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.PerfilAutorId = entrada.PerfilAutorId;
        entidad.CreadoPorUsuarioId = entrada.CreadoPorUsuarioId;
        entidad.TipoCodigo = entrada.TipoCodigo;
        entidad.Titulo = entrada.Titulo;
        entidad.Descripcion = entrada.Descripcion;
        entidad.CategoriaId = entrada.CategoriaId;
        entidad.CiudadId = entrada.CiudadId;
        entidad.EsRemoto = entrada.EsRemoto;
        entidad.Estado = entrada.Estado;
        entidad.ExpiraEn = entrada.ExpiraEn;
        entidad.TotalPostulaciones = entrada.TotalPostulaciones;
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

    private static RespuestaOportunidadDto ARespuesta(Oportunidad entidad) => new()
    {
        Id = entidad.Id,
        PerfilAutorId = entidad.PerfilAutorId,
        CreadoPorUsuarioId = entidad.CreadoPorUsuarioId,
        TipoCodigo = entidad.TipoCodigo,
        Titulo = entidad.Titulo,
        Descripcion = entidad.Descripcion,
        CategoriaId = entidad.CategoriaId,
        CiudadId = entidad.CiudadId,
        EsRemoto = entidad.EsRemoto,
        Estado = entidad.Estado,
        ExpiraEn = entidad.ExpiraEn,
        TotalPostulaciones = entidad.TotalPostulaciones,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn,
        EliminadoEn = entidad.EliminadoEn
    };
}
