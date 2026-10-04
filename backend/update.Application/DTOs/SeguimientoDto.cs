using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para seguimientos; no expone navegaciones.</summary>
public sealed class CrearSeguimientoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioSeguidorId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilSeguidoId { get; set; }
    [EnumDataType(typeof(EstadoSeguimiento))]
    public EstadoSeguimiento Estado { get; set; } = EstadoSeguimiento.Activo;
    [FechaUtc]
    public DateTime? AceptadoEn { get; set; }
}

/// <summary>Contrato actualizar para seguimientos; no expone navegaciones.</summary>
public sealed class ActualizarSeguimientoDto
{
    [EnumDataType(typeof(EstadoSeguimiento))]
    public EstadoSeguimiento Estado { get; set; } = EstadoSeguimiento.Activo;
    [FechaUtc]
    public DateTime? AceptadoEn { get; set; }
}

/// <summary>Contrato respuesta para seguimientos; no expone navegaciones.</summary>
public sealed class RespuestaSeguimientoDto
{
    public Guid UsuarioSeguidorId { get; set; }
    public Guid PerfilSeguidoId { get; set; }
    public EstadoSeguimiento Estado { get; set; } = EstadoSeguimiento.Activo;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? AceptadoEn { get; set; }
}

