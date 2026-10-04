using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para historial_solicitudes; no expone navegaciones.</summary>
public sealed class CrearHistorialSolicitudDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid SolicitudCotizacionId { get; set; }
    [EnumDataType(typeof(EstadoSolicitud))]
    public EstadoSolicitud? EstadoAnterior { get; set; }
    [JsonRequired]
    [EnumDataType(typeof(EstadoSolicitud))]
    public EstadoSolicitud EstadoNuevo { get; set; }
    [GuidNoVacio]
    public Guid? UsuarioActorId { get; set; }
    [MaxLength(4000)]
    public string? Nota { get; set; }
}

/// <summary>Contrato actualizar para historial_solicitudes; no expone navegaciones.</summary>
public sealed class ActualizarHistorialSolicitudDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid SolicitudCotizacionId { get; set; }
    [EnumDataType(typeof(EstadoSolicitud))]
    public EstadoSolicitud? EstadoAnterior { get; set; }
    [JsonRequired]
    [EnumDataType(typeof(EstadoSolicitud))]
    public EstadoSolicitud EstadoNuevo { get; set; }
    [GuidNoVacio]
    public Guid? UsuarioActorId { get; set; }
    [MaxLength(4000)]
    public string? Nota { get; set; }
}

/// <summary>Contrato respuesta para historial_solicitudes; no expone navegaciones.</summary>
public sealed class RespuestaHistorialSolicitudDto
{
    public long Id { get; set; }
    public Guid SolicitudCotizacionId { get; set; }
    public EstadoSolicitud? EstadoAnterior { get; set; }
    public EstadoSolicitud EstadoNuevo { get; set; }
    public Guid? UsuarioActorId { get; set; }
    public string? Nota { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
}

