using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para solicitudes_mentoria; no expone navegaciones.</summary>
public sealed class CrearSolicitudMentoriaDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid OfertaId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilMentoreadoId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Mensaje { get; set; } = string.Empty;
    [EnumDataType(typeof(EstadoMentoria))]
    public EstadoMentoria Estado { get; set; } = EstadoMentoria.Solicitado;
    [FechaUtc]
    public DateTime? AgendadoEn { get; set; }
    [MaxLength(2048)]
    public string? UrlReunion { get; set; }
    public short? CalificacionMentoreado { get; set; }
    [MaxLength(4000)]
    public string? ComentarioMentoreado { get; set; }
}

/// <summary>Contrato actualizar para solicitudes_mentoria; no expone navegaciones.</summary>
public sealed class ActualizarSolicitudMentoriaDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid OfertaId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilMentoreadoId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Mensaje { get; set; } = string.Empty;
    [EnumDataType(typeof(EstadoMentoria))]
    public EstadoMentoria Estado { get; set; } = EstadoMentoria.Solicitado;
    [FechaUtc]
    public DateTime? AgendadoEn { get; set; }
    [MaxLength(2048)]
    public string? UrlReunion { get; set; }
    public short? CalificacionMentoreado { get; set; }
    [MaxLength(4000)]
    public string? ComentarioMentoreado { get; set; }
}

/// <summary>Contrato respuesta para solicitudes_mentoria; no expone navegaciones.</summary>
public sealed class RespuestaSolicitudMentoriaDto
{
    public Guid Id { get; set; }
    public Guid OfertaId { get; set; }
    public Guid PerfilMentoreadoId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public EstadoMentoria Estado { get; set; } = EstadoMentoria.Solicitado;
    public DateTime? AgendadoEn { get; set; }
    public string? UrlReunion { get; set; }
    public short? CalificacionMentoreado { get; set; }
    public string? ComentarioMentoreado { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
}

