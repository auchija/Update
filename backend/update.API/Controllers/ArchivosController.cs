using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using update.Application.DTOs;
using update.Application.Services;
using update.Domain.Entities;
using update.Domain.Enums;
namespace update.API.Controllers;

/// <summary>CRUD administrativo de archivos; la autorización de usuario final debe añadirse por caso de uso.</summary>
[ApiController]
[Authorize(Policy = "Administracion")]
[Route("api/archivos")]
public sealed class ArchivosController(ServicioCrud<Archivo> servicio) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RespuestaArchivoDto>>> Listar([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 20, CancellationToken cancellationToken = default)
    {
        var entidades = await servicio.ListarAsync(pagina, tamanoPagina, cancellationToken);
        return Ok(entidades.Select(ARespuesta).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RespuestaArchivoDto>> Obtener(Guid id, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        return Ok(ARespuesta(entidad));
    }

    [HttpPost]
    public async Task<ActionResult<RespuestaArchivoDto>> Crear([FromBody] CrearArchivoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = new Archivo
        {
            SubidoPorUsuarioId = entrada.SubidoPorUsuarioId,
            Tipo = entrada.Tipo,
            Proposito = entrada.Proposito,
            ProveedorAlmacenamiento = entrada.ProveedorAlmacenamiento,
            Bucket = entrada.Bucket,
            ClaveObjeto = entrada.ClaveObjeto,
            NombreOriginal = entrada.NombreOriginal,
            TipoMime = entrada.TipoMime,
            Extension = entrada.Extension,
            TamanoBytes = entrada.TamanoBytes,
            ChecksumSha256 = entrada.ChecksumSha256,
            Ancho = entrada.Ancho,
            Alto = entrada.Alto,
            DuracionSegundos = entrada.DuracionSegundos,
            Visibilidad = entrada.Visibilidad,
            Estado = entrada.Estado
        };
        await servicio.CrearAsync(entidad, cancellationToken);
        return CreatedAtAction(nameof(Obtener), new { id = entidad.Id }, ARespuesta(entidad));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarArchivoDto entrada, CancellationToken cancellationToken)
    {
        var entidad = await servicio.ObtenerAsync([id], cancellationToken);
        if (entidad is null) return NotFound(new ProblemDetails { Status = 404, Title = "Registro no encontrado." });
        entidad.SubidoPorUsuarioId = entrada.SubidoPorUsuarioId;
        entidad.Tipo = entrada.Tipo;
        entidad.Proposito = entrada.Proposito;
        entidad.ProveedorAlmacenamiento = entrada.ProveedorAlmacenamiento;
        entidad.Bucket = entrada.Bucket;
        entidad.ClaveObjeto = entrada.ClaveObjeto;
        entidad.NombreOriginal = entrada.NombreOriginal;
        entidad.TipoMime = entrada.TipoMime;
        entidad.Extension = entrada.Extension;
        entidad.TamanoBytes = entrada.TamanoBytes;
        entidad.ChecksumSha256 = entrada.ChecksumSha256;
        entidad.Ancho = entrada.Ancho;
        entidad.Alto = entrada.Alto;
        entidad.DuracionSegundos = entrada.DuracionSegundos;
        entidad.Visibilidad = entrada.Visibilidad;
        entidad.Estado = entrada.Estado;
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

    private static RespuestaArchivoDto ARespuesta(Archivo entidad) => new()
    {
        Id = entidad.Id,
        SubidoPorUsuarioId = entidad.SubidoPorUsuarioId,
        Tipo = entidad.Tipo,
        Proposito = entidad.Proposito,
        ProveedorAlmacenamiento = entidad.ProveedorAlmacenamiento,
        Bucket = entidad.Bucket,
        ClaveObjeto = entidad.ClaveObjeto,
        NombreOriginal = entidad.NombreOriginal,
        TipoMime = entidad.TipoMime,
        Extension = entidad.Extension,
        TamanoBytes = entidad.TamanoBytes,
        ChecksumSha256 = entidad.ChecksumSha256,
        Ancho = entidad.Ancho,
        Alto = entidad.Alto,
        DuracionSegundos = entidad.DuracionSegundos,
        Visibilidad = entidad.Visibilidad,
        Estado = entidad.Estado,
        CreadoEn = entidad.CreadoEn,
        ActualizadoEn = entidad.ActualizadoEn,
        EliminadoEn = entidad.EliminadoEn
    };
}
