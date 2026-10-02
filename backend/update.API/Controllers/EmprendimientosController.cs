using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de emprendimientos; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/emprendimientos")]
public sealed class EmprendimientosController(ServicioCrud<Emprendimiento> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaEmprendimientoDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaEmprendimientoDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaEmprendimientoDto>> Crear([FromBody] CrearEmprendimientoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Emprendimiento
        {
            Id = entrada.Id,
            TipoPerfil = entrada.TipoPerfil,
            CategoriaId = entrada.CategoriaId,
            Etapa = entrada.Etapa,
            FundadoEn = entrada.FundadoEn,
            RazonSocial = entrada.RazonSocial,
            Nit = entrada.Nit,
            CorreoContacto = entrada.CorreoContacto,
            TelefonoContacto = entrada.TelefonoContacto,
            Whatsapp = entrada.Whatsapp,
            Direccion = entrada.Direccion,
            EnviosNacionales = entrada.EnviosNacionales,
            OfreceRemoto = entrada.OfreceRemoto,
            EstadoVerificacion = entrada.EstadoVerificacion,
            VerificadoEn = entrada.VerificadoEn,
            CalificacionPromedio = entrada.CalificacionPromedio,
            TotalCalificaciones = entrada.TotalCalificaciones,
            CreadoPorUsuarioId = entrada.CreadoPorUsuarioId
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarEmprendimientoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.TipoPerfil = entrada.TipoPerfil;
        entidad.CategoriaId = entrada.CategoriaId;
        entidad.Etapa = entrada.Etapa;
        entidad.FundadoEn = entrada.FundadoEn;
        entidad.RazonSocial = entrada.RazonSocial;
        entidad.Nit = entrada.Nit;
        entidad.CorreoContacto = entrada.CorreoContacto;
        entidad.TelefonoContacto = entrada.TelefonoContacto;
        entidad.Whatsapp = entrada.Whatsapp;
        entidad.Direccion = entrada.Direccion;
        entidad.EnviosNacionales = entrada.EnviosNacionales;
        entidad.OfreceRemoto = entrada.OfreceRemoto;
        entidad.EstadoVerificacion = entrada.EstadoVerificacion;
        entidad.VerificadoEn = entrada.VerificadoEn;
        entidad.CalificacionPromedio = entrada.CalificacionPromedio;
        entidad.TotalCalificaciones = entrada.TotalCalificaciones;
        entidad.CreadoPorUsuarioId = entrada.CreadoPorUsuarioId;
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

    private static RespuestaEmprendimientoDto ARespuesta(Emprendimiento entidad) => new()
    {
        Id = entidad.Id,
        TipoPerfil = entidad.TipoPerfil,
        CategoriaId = entidad.CategoriaId,
        Etapa = entidad.Etapa,
        FundadoEn = entidad.FundadoEn,
        RazonSocial = entidad.RazonSocial,
        Nit = entidad.Nit,
        CorreoContacto = entidad.CorreoContacto,
        TelefonoContacto = entidad.TelefonoContacto,
        Whatsapp = entidad.Whatsapp,
        Direccion = entidad.Direccion,
        EnviosNacionales = entidad.EnviosNacionales,
        OfreceRemoto = entidad.OfreceRemoto,
        EstadoVerificacion = entidad.EstadoVerificacion,
        VerificadoEn = entidad.VerificadoEn,
        CalificacionPromedio = entidad.CalificacionPromedio,
        TotalCalificaciones = entidad.TotalCalificaciones,
        CreadoPorUsuarioId = entidad.CreadoPorUsuarioId,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn
    };
}
