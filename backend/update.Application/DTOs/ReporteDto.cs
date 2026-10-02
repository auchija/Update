using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para reportes; no expone navegaciones.</summary>
public sealed class CrearReporteDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioReportanteId { get; set; }
    [JsonRequired]
    [EnumDataType(typeof(ObjetivoReporte))]
    public ObjetivoReporte ObjetivoTipo { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid ObjetivoId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string MotivoCodigo { get; set; } = string.Empty;
    [MaxLength(4000)]
    public string? Detalles { get; set; }
    [EnumDataType(typeof(EstadoReporte))]
    public EstadoReporte Estado { get; set; } = EstadoReporte.Abierto;
    [GuidNoVacio]
    public Guid? ResueltoPorUsuarioId { get; set; }
    [MaxLength(4000)]
    public string? NotaResolucion { get; set; }
    [FechaUtc]
    public DateTime? ResueltoEn { get; set; }
}

/// <summary>Contrato actualizar para reportes; no expone navegaciones.</summary>
public sealed class ActualizarReporteDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioReportanteId { get; set; }
    [JsonRequired]
    [EnumDataType(typeof(ObjetivoReporte))]
    public ObjetivoReporte ObjetivoTipo { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid ObjetivoId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string MotivoCodigo { get; set; } = string.Empty;
    [MaxLength(4000)]
    public string? Detalles { get; set; }
    [EnumDataType(typeof(EstadoReporte))]
    public EstadoReporte Estado { get; set; } = EstadoReporte.Abierto;
    [GuidNoVacio]
    public Guid? ResueltoPorUsuarioId { get; set; }
    [MaxLength(4000)]
    public string? NotaResolucion { get; set; }
    [FechaUtc]
    public DateTime? ResueltoEn { get; set; }
}

/// <summary>Contrato respuesta para reportes; no expone navegaciones.</summary>
public sealed class RespuestaReporteDto
{
    public Guid Id { get; set; }
    public Guid UsuarioReportanteId { get; set; }
    public ObjetivoReporte ObjetivoTipo { get; set; }
    public Guid ObjetivoId { get; set; }
    public string MotivoCodigo { get; set; } = string.Empty;
    public string? Detalles { get; set; }
    public EstadoReporte Estado { get; set; } = EstadoReporte.Abierto;
    public Guid? ResueltoPorUsuarioId { get; set; }
    public string? NotaResolucion { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? ResueltoEn { get; set; }
}

