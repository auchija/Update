using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de perfiles; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/perfiles")]
public sealed class PerfilesController(ServicioCrud<Perfil> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaPerfilDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaPerfilDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaPerfilDto>> Crear([FromBody] CrearPerfilDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Perfil
        {
            Tipo = entrada.Tipo,
            NombreUsuario = entrada.NombreUsuario,
            NombreVisible = entrada.NombreVisible,
            Titular = entrada.Titular,
            Biografia = entrada.Biografia,
            FotoArchivoId = entrada.FotoArchivoId,
            PortadaArchivoId = entrada.PortadaArchivoId,
            CiudadId = entrada.CiudadId,
            SitioWeb = entrada.SitioWeb,
            EsPrivado = entrada.EsPrivado,
            Estado = entrada.Estado,
            TotalSeguidores = entrada.TotalSeguidores,
            TotalSeguidos = entrada.TotalSeguidos,
            TotalPublicaciones = entrada.TotalPublicaciones
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarPerfilDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.NombreUsuario = entrada.NombreUsuario;
        entidad.NombreVisible = entrada.NombreVisible;
        entidad.Titular = entrada.Titular;
        entidad.Biografia = entrada.Biografia;
        entidad.FotoArchivoId = entrada.FotoArchivoId;
        entidad.PortadaArchivoId = entrada.PortadaArchivoId;
        entidad.CiudadId = entrada.CiudadId;
        entidad.SitioWeb = entrada.SitioWeb;
        entidad.EsPrivado = entrada.EsPrivado;
        entidad.Estado = entrada.Estado;
        entidad.TotalSeguidores = entrada.TotalSeguidores;
        entidad.TotalSeguidos = entrada.TotalSeguidos;
        entidad.TotalPublicaciones = entrada.TotalPublicaciones;
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

    private static RespuestaPerfilDto ARespuesta(Perfil entidad) => new()
    {
        Id = entidad.Id,
        Tipo = entidad.Tipo,
        NombreUsuario = entidad.NombreUsuario,
        NombreVisible = entidad.NombreVisible,
        Titular = entidad.Titular,
        Biografia = entidad.Biografia,
        FotoArchivoId = entidad.FotoArchivoId,
        PortadaArchivoId = entidad.PortadaArchivoId,
        CiudadId = entidad.CiudadId,
        SitioWeb = entidad.SitioWeb,
        EsPrivado = entidad.EsPrivado,
        Estado = entidad.Estado,
        TotalSeguidores = entidad.TotalSeguidores,
        TotalSeguidos = entidad.TotalSeguidos,
        TotalPublicaciones = entidad.TotalPublicaciones,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn,
        EliminadoEn = entidad.EliminadoEn
    };
}
