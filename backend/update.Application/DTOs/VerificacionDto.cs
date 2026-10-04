using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para verificaciones; no expone navegaciones.</summary>
public sealed class CrearVerificacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid EmprendimientoId { get; set; }
    [EnumDataType(typeof(EstadoVerificacion))]
    public EstadoVerificacion Estado { get; set; } = EstadoVerificacion.Pendiente;
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string Metodo { get; set; } = "documentos";
    [JsonRequired]
    [GuidNoVacio]
    public Guid EnviadoPorUsuarioId { get; set; }
    [GuidNoVacio]
    public Guid? RevisadoPorUsuarioId { get; set; }
    [MaxLength(4000)]
    public string? NotasRevisor { get; set; }
    [FechaUtc]
    public DateTime? RevisadoEn { get; set; }
}

/// <summary>Contrato actualizar para verificaciones; no expone navegaciones.</summary>
public sealed class ActualizarVerificacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid EmprendimientoId { get; set; }
    [EnumDataType(typeof(EstadoVerificacion))]
    public EstadoVerificacion Estado { get; set; } = EstadoVerificacion.Pendiente;
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string Metodo { get; set; } = "documentos";
    [JsonRequired]
    [GuidNoVacio]
    public Guid EnviadoPorUsuarioId { get; set; }
    [GuidNoVacio]
    public Guid? RevisadoPorUsuarioId { get; set; }
    [MaxLength(4000)]
    public string? NotasRevisor { get; set; }
    [FechaUtc]
    public DateTime? RevisadoEn { get; set; }
}

/// <summary>Contrato respuesta para verificaciones; no expone navegaciones.</summary>
public sealed class RespuestaVerificacionDto
{
    public Guid Id { get; set; }
    public Guid EmprendimientoId { get; set; }
    public EstadoVerificacion Estado { get; set; } = EstadoVerificacion.Pendiente;
    public string Metodo { get; set; } = "documentos";
    public Guid EnviadoPorUsuarioId { get; set; }
    public Guid? RevisadoPorUsuarioId { get; set; }
    public string? NotasRevisor { get; set; }
    public DateTime EnviadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? RevisadoEn { get; set; }
}

