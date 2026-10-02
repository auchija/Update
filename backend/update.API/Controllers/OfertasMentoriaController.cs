using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de ofertas_mentoria; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/ofertas_mentoria")]
public sealed class OfertasMentoriaController(ServicioCrud<OfertaMentoria> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaOfertaMentoriaDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaOfertaMentoriaDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaOfertaMentoriaDto>> Crear([FromBody] CrearOfertaMentoriaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new OfertaMentoria
        {
            UsuarioMentorId = entrada.UsuarioMentorId,
            Titulo = entrada.Titulo,
            Descripcion = entrada.Descripcion,
            CategoriaId = entrada.CategoriaId,
            Modalidad = entrada.Modalidad,
            DuracionMinutos = entrada.DuracionMinutos,
            EsGratis = entrada.EsGratis,
            Precio = entrada.Precio,
            MonedaCodigo = entrada.MonedaCodigo,
            Activo = entrada.Activo
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarOfertaMentoriaDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.UsuarioMentorId = entrada.UsuarioMentorId;
        entidad.Titulo = entrada.Titulo;
        entidad.Descripcion = entrada.Descripcion;
        entidad.CategoriaId = entrada.CategoriaId;
        entidad.Modalidad = entrada.Modalidad;
        entidad.DuracionMinutos = entrada.DuracionMinutos;
        entidad.EsGratis = entrada.EsGratis;
        entidad.Precio = entrada.Precio;
        entidad.MonedaCodigo = entrada.MonedaCodigo;
        entidad.Activo = entrada.Activo;
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

    private static RespuestaOfertaMentoriaDto ARespuesta(OfertaMentoria entidad) => new()
    {
        Id = entidad.Id,
        UsuarioMentorId = entidad.UsuarioMentorId,
        Titulo = entidad.Titulo,
        Descripcion = entidad.Descripcion,
        CategoriaId = entidad.CategoriaId,
        Modalidad = entidad.Modalidad,
        DuracionMinutos = entidad.DuracionMinutos,
        EsGratis = entidad.EsGratis,
        Precio = entidad.Precio,
        MonedaCodigo = entidad.MonedaCodigo,
        Activo = entidad.Activo,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn
    };
}
