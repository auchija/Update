using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para solicitudes_cotizacion; no expone navegaciones.</summary>
public sealed class CrearSolicitudCotizacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid EmprendimientoId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilSolicitanteId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [GuidNoVacio]
    public Guid? ConversacionId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Titulo { get; set; } = string.Empty;
    [MaxLength(10000)]
    public string? Mensaje { get; set; }
    public DateOnly? FechaDeseada { get; set; }
    public int? CiudadEntregaId { get; set; }
    [EnumDataType(typeof(EstadoSolicitud))]
    public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;
    [GuidNoVacio]
    public Guid? CotizacionAceptadaId { get; set; }
    [FechaUtc]
    public DateTime? CerradoEn { get; set; }
}

/// <summary>Contrato actualizar para solicitudes_cotizacion; no expone navegaciones.</summary>
public sealed class ActualizarSolicitudCotizacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid EmprendimientoId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilSolicitanteId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [GuidNoVacio]
    public Guid? ConversacionId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Titulo { get; set; } = string.Empty;
    [MaxLength(10000)]
    public string? Mensaje { get; set; }
    public DateOnly? FechaDeseada { get; set; }
    public int? CiudadEntregaId { get; set; }
    [EnumDataType(typeof(EstadoSolicitud))]
    public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;
    [GuidNoVacio]
    public Guid? CotizacionAceptadaId { get; set; }
    [FechaUtc]
    public DateTime? CerradoEn { get; set; }
}

/// <summary>Contrato respuesta para solicitudes_cotizacion; no expone navegaciones.</summary>
public sealed class RespuestaSolicitudCotizacionDto
{
    public Guid Id { get; set; }
    public long Numero { get; set; }
    public Guid EmprendimientoId { get; set; }
    public Guid PerfilSolicitanteId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public Guid? ConversacionId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Mensaje { get; set; }
    public DateOnly? FechaDeseada { get; set; }
    public int? CiudadEntregaId { get; set; }
    public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;
    public Guid? CotizacionAceptadaId { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? CerradoEn { get; set; }
}

