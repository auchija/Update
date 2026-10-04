using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para guardados; no expone navegaciones.</summary>
public sealed class CrearGuardadoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioId { get; set; }
    [GuidNoVacio]
    public Guid? PublicacionId { get; set; }
    [GuidNoVacio]
    public Guid? ProductoId { get; set; }
}

/// <summary>Contrato actualizar para guardados; no expone navegaciones.</summary>
public sealed class ActualizarGuardadoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioId { get; set; }
    [GuidNoVacio]
    public Guid? PublicacionId { get; set; }
    [GuidNoVacio]
    public Guid? ProductoId { get; set; }
}

/// <summary>Contrato respuesta para guardados; no expone navegaciones.</summary>
public sealed class RespuestaGuardadoDto
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid? PublicacionId { get; set; }
    public Guid? ProductoId { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
}

