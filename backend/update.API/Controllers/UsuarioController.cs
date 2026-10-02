using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de usuarios; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/usuarios")]
[Route("api/Users")]
public sealed class UsuarioController(ServicioCrud<Usuario> servicio, update.Domain.Interfaces.IUsuarioRepository repositorio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaUsuarioDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaUsuarioDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpGet("correo/{correo}")]
    public async Task<ActionResult<RespuestaUsuarioDto>> ObtenerPorCorreo(string correo, CancellationToken cancellationToken)
    {
        var entidad = await repositorio.ObtenerPorCorreoAsync(correo, cancellationToken);
        return entidad is null ? NotFound(new ProblemDetails { Status = 404, Title = "Usuario no encontrado." }) : Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaUsuarioDto>> Crear([FromBody] CrearUsuarioDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Usuario
        {
            Id = entrada.Id,
            TipoPerfil = entrada.TipoPerfil,
            Correo = entrada.Correo,
            HashContrasena = entrada.HashContrasena,
            RolPlataforma = entrada.RolPlataforma,
            CorreoVerificadoEn = entrada.CorreoVerificadoEn,
            TerminosAceptadosEn = entrada.TerminosAceptadosEn,
            UltimoIngresoEn = entrada.UltimoIngresoEn,
            IntentosFallidos = entrada.IntentosFallidos,
            BloqueadoHasta = entrada.BloqueadoHasta,
            ContrasenaCambiadaEn = entrada.ContrasenaCambiadaEn
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarUsuarioDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.TipoPerfil = entrada.TipoPerfil;
        entidad.Correo = entrada.Correo;
        entidad.HashContrasena = entrada.HashContrasena;
        entidad.RolPlataforma = entrada.RolPlataforma;
        entidad.CorreoVerificadoEn = entrada.CorreoVerificadoEn;
        entidad.TerminosAceptadosEn = entrada.TerminosAceptadosEn;
        entidad.UltimoIngresoEn = entrada.UltimoIngresoEn;
        entidad.IntentosFallidos = entrada.IntentosFallidos;
        entidad.BloqueadoHasta = entrada.BloqueadoHasta;
        entidad.ContrasenaCambiadaEn = entrada.ContrasenaCambiadaEn;
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

    private static RespuestaUsuarioDto ARespuesta(Usuario entidad) => new()
    {
        Id = entidad.Id,
        TipoPerfil = entidad.TipoPerfil,
        Correo = entidad.Correo,
        RolPlataforma = entidad.RolPlataforma,
        CorreoVerificadoEn = entidad.CorreoVerificadoEn,
        TerminosAceptadosEn = entidad.TerminosAceptadosEn,
        UltimoIngresoEn = entidad.UltimoIngresoEn,
        IntentosFallidos = entidad.IntentosFallidos,
        BloqueadoHasta = entidad.BloqueadoHasta,
        ContrasenaCambiadaEn = entidad.ContrasenaCambiadaEn,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn
    };
}
