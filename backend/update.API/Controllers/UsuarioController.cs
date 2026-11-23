using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Domain.Entities;
using update.Domain.Interfaces;

namespace update.API.Controllers;

[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/usuarios")]
public sealed class UsuarioController(IServicioUsuario servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaUsuarioDto>>> Listar(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 20,
        CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaUsuarioDto>> Obtener(
        Guid id,
        CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerPorIdAsync(id, cancellationToken);

        if (entidad is null)
            return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = "Registro no encontrado."
            });

        return Ok(ARespuesta(entidad));
    }

    [HttpGet("correo/{correo}")]
    public async Task<ActionResult<RespuestaUsuarioDto>> ObtenerPorCorreo(
        string correo,
        CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerPorCorreoAsync(correo, cancellationToken);

        if (entidad is null)
            return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = "Usuario no encontrado."
            });

        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaUsuarioDto>> Crear(
        [FromBody] CrearUsuarioDto entrada,
        CancellationToken cancellationToken)
    {
        var entidad = new Usuario
        {
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

        entidad = await servicio.CrearAsync(entidad, cancellationToken);

        return CreatedAtAction(
            nameof(Obtener),
            new { id = entidad.Id },
            ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(
        Guid id,
        [FromBody] ActualizarUsuarioDto entrada,
        CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerPorIdAsync(id, cancellationToken);

        if (entidad is null)
            return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = "Registro no encontrado."
            });

        entidad.Correo = entrada.Correo;
        entidad.HashContrasena = entrada.HashContrasena;
        entidad.RolPlataforma = entrada.RolPlataforma;
        entidad.CorreoVerificadoEn = entrada.CorreoVerificadoEn;
        entidad.TerminosAceptadosEn = entrada.TerminosAceptadosEn;
        entidad.UltimoIngresoEn = entrada.UltimoIngresoEn;
        entidad.IntentosFallidos = entrada.IntentosFallidos;
        entidad.BloqueadoHasta = entrada.BloqueadoHasta;
        entidad.ContrasenaCambiadaEn = entrada.ContrasenaCambiadaEn;

        await servicio.ActualizarAsync(id, entidad, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(
        Guid id,
        CancellationToken cancellationToken)
    {
        if (!await servicio.EliminarAsync(id, cancellationToken))
            return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = "Registro no encontrado."
            });

        return NoContent();
    }

    private static RespuestaUsuarioDto ARespuesta(Usuario entidad) => new()
    {
        Id = entidad.Id,
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
