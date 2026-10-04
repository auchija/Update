using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para conversaciones; no expone navegaciones.</summary>
public sealed class CrearConversacionDto
{
    [EnumDataType(typeof(TipoConversacion))]
    public TipoConversacion Tipo { get; set; } = TipoConversacion.Directa;
    [MaxLength(255)]
    public string? Titulo { get; set; }
    [MaxLength(1024)]
    public string? ClaveDirecta { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [GuidNoVacio]
    public Guid? UltimoMensajeId { get; set; }
    [FechaUtc]
    public DateTime? UltimoMensajeEn { get; set; }
}

/// <summary>Contrato actualizar para conversaciones; no expone navegaciones.</summary>
public sealed class ActualizarConversacionDto
{
    [EnumDataType(typeof(TipoConversacion))]
    public TipoConversacion Tipo { get; set; } = TipoConversacion.Directa;
    [MaxLength(255)]
    public string? Titulo { get; set; }
    [MaxLength(1024)]
    public string? ClaveDirecta { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [GuidNoVacio]
    public Guid? UltimoMensajeId { get; set; }
    [FechaUtc]
    public DateTime? UltimoMensajeEn { get; set; }
}

/// <summary>Contrato respuesta para conversaciones; no expone navegaciones.</summary>
public sealed class RespuestaConversacionDto
{
    public Guid Id { get; set; }
    public TipoConversacion Tipo { get; set; } = TipoConversacion.Directa;
    public string? Titulo { get; set; }
    public string? ClaveDirecta { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public Guid? UltimoMensajeId { get; set; }
    public DateTime? UltimoMensajeEn { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
}

