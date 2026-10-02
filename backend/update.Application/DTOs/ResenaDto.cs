using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para resenas; no expone navegaciones.</summary>
public sealed class CrearResenaDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioResenadorId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid EmprendimientoId { get; set; }
    [GuidNoVacio]
    public Guid? ProductoId { get; set; }
    [GuidNoVacio]
    public Guid? SolicitudCotizacionId { get; set; }
    [JsonRequired]
    public short Calificacion { get; set; }
    [MaxLength(255)]
    public string? Titulo { get; set; }
    [MaxLength(10000)]
    public string? Contenido { get; set; }
    [EnumDataType(typeof(EstadoResena))]
    public EstadoResena Estado { get; set; } = EstadoResena.Publicado;
    [MaxLength(10000)]
    public string? RespuestaEmprendimiento { get; set; }
    [GuidNoVacio]
    public Guid? RespondidoPorUsuarioId { get; set; }
    [FechaUtc]
    public DateTime? RespondidoEn { get; set; }
}

/// <summary>Contrato actualizar para resenas; no expone navegaciones.</summary>
public sealed class ActualizarResenaDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioResenadorId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid EmprendimientoId { get; set; }
    [GuidNoVacio]
    public Guid? ProductoId { get; set; }
    [GuidNoVacio]
    public Guid? SolicitudCotizacionId { get; set; }
    [JsonRequired]
    public short Calificacion { get; set; }
    [MaxLength(255)]
    public string? Titulo { get; set; }
    [MaxLength(10000)]
    public string? Contenido { get; set; }
    [EnumDataType(typeof(EstadoResena))]
    public EstadoResena Estado { get; set; } = EstadoResena.Publicado;
    [MaxLength(10000)]
    public string? RespuestaEmprendimiento { get; set; }
    [GuidNoVacio]
    public Guid? RespondidoPorUsuarioId { get; set; }
    [FechaUtc]
    public DateTime? RespondidoEn { get; set; }
}

/// <summary>Contrato respuesta para resenas; no expone navegaciones.</summary>
public sealed class RespuestaResenaDto
{
    public Guid Id { get; set; }
    public Guid UsuarioResenadorId { get; set; }
    public Guid EmprendimientoId { get; set; }
    public Guid? ProductoId { get; set; }
    public Guid? SolicitudCotizacionId { get; set; }
    public short Calificacion { get; set; }
    public string? Titulo { get; set; }
    public string? Contenido { get; set; }
    public EstadoResena Estado { get; set; } = EstadoResena.Publicado;
    public string? RespuestaEmprendimiento { get; set; }
    public Guid? RespondidoPorUsuarioId { get; set; }
    public DateTime? RespondidoEn { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? EliminadoEn { get; set; }
}

