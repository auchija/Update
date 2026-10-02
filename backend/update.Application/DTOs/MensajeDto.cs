using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para mensajes; no expone navegaciones.</summary>
public sealed class CrearMensajeDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid ConversacionId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilRemitenteId { get; set; }
    [GuidNoVacio]
    public Guid? UsuarioRemitenteId { get; set; }
    [EnumDataType(typeof(TipoMensaje))]
    public TipoMensaje Tipo { get; set; } = TipoMensaje.Texto;
    [MaxLength(10000)]
    public string? Contenido { get; set; }
    [GuidNoVacio]
    public Guid? ProductoId { get; set; }
    [GuidNoVacio]
    public Guid? SolicitudCotizacionId { get; set; }
    [GuidNoVacio]
    public Guid? RespondeAMensajeId { get; set; }
}

/// <summary>Contrato actualizar para mensajes; no expone navegaciones.</summary>
public sealed class ActualizarMensajeDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid ConversacionId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilRemitenteId { get; set; }
    [GuidNoVacio]
    public Guid? UsuarioRemitenteId { get; set; }
    [EnumDataType(typeof(TipoMensaje))]
    public TipoMensaje Tipo { get; set; } = TipoMensaje.Texto;
    [MaxLength(10000)]
    public string? Contenido { get; set; }
    [GuidNoVacio]
    public Guid? ProductoId { get; set; }
    [GuidNoVacio]
    public Guid? SolicitudCotizacionId { get; set; }
    [GuidNoVacio]
    public Guid? RespondeAMensajeId { get; set; }
}

/// <summary>Contrato respuesta para mensajes; no expone navegaciones.</summary>
public sealed class RespuestaMensajeDto
{
    public Guid Id { get; set; }
    public Guid ConversacionId { get; set; }
    public Guid PerfilRemitenteId { get; set; }
    public Guid? UsuarioRemitenteId { get; set; }
    public TipoMensaje Tipo { get; set; } = TipoMensaje.Texto;
    public string? Contenido { get; set; }
    public Guid? ProductoId { get; set; }
    public Guid? SolicitudCotizacionId { get; set; }
    public Guid? RespondeAMensajeId { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? EditadoEn { get; set; }
    public DateTime? EliminadoEn { get; set; }
}

