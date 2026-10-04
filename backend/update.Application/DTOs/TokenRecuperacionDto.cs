using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para tokens_recuperacion; no expone navegaciones.</summary>
public sealed class CrearTokenRecuperacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(256)]
    public string HashToken { get; set; } = string.Empty;
    public IPAddress? IpSolicitud { get; set; }
    [JsonRequired]
    [FechaUtc]
    public DateTime ExpiraEn { get; set; }
    [FechaUtc]
    public DateTime? UsadoEn { get; set; }
}

/// <summary>Contrato actualizar para tokens_recuperacion; no expone navegaciones.</summary>
public sealed class ActualizarTokenRecuperacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(256)]
    public string HashToken { get; set; } = string.Empty;
    public IPAddress? IpSolicitud { get; set; }
    [JsonRequired]
    [FechaUtc]
    public DateTime ExpiraEn { get; set; }
    [FechaUtc]
    public DateTime? UsadoEn { get; set; }
}

/// <summary>Contrato respuesta para tokens_recuperacion; no expone navegaciones.</summary>
public sealed class RespuestaTokenRecuperacionDto
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public IPAddress? IpSolicitud { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ExpiraEn { get; set; }
    public DateTime? UsadoEn { get; set; }
}

