using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para enlaces_emprendimiento; no expone navegaciones.</summary>
public sealed class CrearEnlaceEmprendimientoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid EmprendimientoId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string Plataforma { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(2048)]
    public string Url { get; set; } = string.Empty;
    public short Orden { get; set; } = (short)0;
}

/// <summary>Contrato actualizar para enlaces_emprendimiento; no expone navegaciones.</summary>
public sealed class ActualizarEnlaceEmprendimientoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid EmprendimientoId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string Plataforma { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(2048)]
    public string Url { get; set; } = string.Empty;
    public short Orden { get; set; } = (short)0;
}

/// <summary>Contrato respuesta para enlaces_emprendimiento; no expone navegaciones.</summary>
public sealed class RespuestaEnlaceEmprendimientoDto
{
    public Guid Id { get; set; }
    public Guid EmprendimientoId { get; set; }
    public string Plataforma { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public short Orden { get; set; } = (short)0;
}

