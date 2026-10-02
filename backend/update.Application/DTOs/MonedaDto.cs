using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para monedas; no expone navegaciones.</summary>
public sealed class CrearMonedaDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(3)]
    [MinLength(3)]
    public string Codigo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Simbolo { get; set; } = string.Empty;
    public short Decimales { get; set; } = (short)0;
    public bool Activo { get; set; } = true;
}

/// <summary>Contrato actualizar para monedas; no expone navegaciones.</summary>
public sealed class ActualizarMonedaDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Simbolo { get; set; } = string.Empty;
    public short Decimales { get; set; } = (short)0;
    public bool Activo { get; set; } = true;
}

/// <summary>Contrato respuesta para monedas; no expone navegaciones.</summary>
public sealed class RespuestaMonedaDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Simbolo { get; set; } = string.Empty;
    public short Decimales { get; set; } = (short)0;
    public bool Activo { get; set; } = true;
}

