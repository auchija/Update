using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para sesiones; no expone navegaciones.</summary>
public sealed class CrearSesionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(256)]
    public string HashTokenRefresco { get; set; } = string.Empty;
    [JsonRequired]
    [GuidNoVacio]
    public Guid FamiliaId { get; set; }
    [GuidNoVacio]
    public Guid? ReemplazadoPorId { get; set; }
    [MaxLength(255)]
    public string? AgenteUsuario { get; set; }
    public IPAddress? DireccionIp { get; set; }
    [JsonRequired]
    [FechaUtc]
    public DateTime ExpiraEn { get; set; }
    [FechaUtc]
    public DateTime? RevocadoEn { get; set; }
    [MaxLength(255)]
    public string? MotivoRevocacion { get; set; }
}

/// <summary>Contrato actualizar para sesiones; no expone navegaciones.</summary>
public sealed class ActualizarSesionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(256)]
    public string HashTokenRefresco { get; set; } = string.Empty;
    [JsonRequired]
    [GuidNoVacio]
    public Guid FamiliaId { get; set; }
    [GuidNoVacio]
    public Guid? ReemplazadoPorId { get; set; }
    [MaxLength(255)]
    public string? AgenteUsuario { get; set; }
    public IPAddress? DireccionIp { get; set; }
    [JsonRequired]
    [FechaUtc]
    public DateTime ExpiraEn { get; set; }
    [FechaUtc]
    public DateTime? RevocadoEn { get; set; }
    [MaxLength(255)]
    public string? MotivoRevocacion { get; set; }
}

/// <summary>Contrato respuesta para sesiones; no expone navegaciones.</summary>
public sealed class RespuestaSesionDto
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid FamiliaId { get; set; }
    public Guid? ReemplazadoPorId { get; set; }
    public string? AgenteUsuario { get; set; }
    public IPAddress? DireccionIp { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime UltimoUsoEn { get; set; } = DateTime.UtcNow;
    public DateTime ExpiraEn { get; set; }
    public DateTime? RevocadoEn { get; set; }
    public string? MotivoRevocacion { get; set; }
}

