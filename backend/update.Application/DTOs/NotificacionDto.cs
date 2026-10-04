using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para notificaciones; no expone navegaciones.</summary>
public sealed class CrearNotificacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioDestinatarioId { get; set; }
    [JsonRequired]
    [EnumDataType(typeof(TipoNotificacion))]
    public TipoNotificacion Tipo { get; set; }
    [GuidNoVacio]
    public Guid? PerfilActorId { get; set; }
    [MaxLength(64)]
    public string? EntidadTipo { get; set; }
    [GuidNoVacio]
    public Guid? EntidadId { get; set; }
    [MaxLength(1024)]
    public string? ClaveGrupo { get; set; }
    public JsonElement Datos { get; set; } = JsonSerializer.Deserialize<JsonElement>("{}");
    [FechaUtc]
    public DateTime? LeidoEn { get; set; }
}

/// <summary>Contrato actualizar para notificaciones; no expone navegaciones.</summary>
public sealed class ActualizarNotificacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioDestinatarioId { get; set; }
    [JsonRequired]
    [EnumDataType(typeof(TipoNotificacion))]
    public TipoNotificacion Tipo { get; set; }
    [GuidNoVacio]
    public Guid? PerfilActorId { get; set; }
    [MaxLength(64)]
    public string? EntidadTipo { get; set; }
    [GuidNoVacio]
    public Guid? EntidadId { get; set; }
    [MaxLength(1024)]
    public string? ClaveGrupo { get; set; }
    public JsonElement Datos { get; set; } = JsonSerializer.Deserialize<JsonElement>("{}");
    [FechaUtc]
    public DateTime? LeidoEn { get; set; }
}

/// <summary>Contrato respuesta para notificaciones; no expone navegaciones.</summary>
public sealed class RespuestaNotificacionDto
{
    public Guid Id { get; set; }
    public Guid UsuarioDestinatarioId { get; set; }
    public TipoNotificacion Tipo { get; set; }
    public Guid? PerfilActorId { get; set; }
    public string? EntidadTipo { get; set; }
    public Guid? EntidadId { get; set; }
    public string? ClaveGrupo { get; set; }
    public JsonElement Datos { get; set; } = JsonSerializer.Deserialize<JsonElement>("{}");
    public DateTime? LeidoEn { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
}

