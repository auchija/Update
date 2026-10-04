using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para ciudades; no expone navegaciones.</summary>
public sealed class CrearCiudadDto
{
    [JsonRequired]
    public int DepartamentoId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [MaxLength(64)]
    public string? Codigo { get; set; }
    [JsonRequired]
    public decimal Latitud { get; set; }
    [JsonRequired]
    public decimal Longitud { get; set; }
}

/// <summary>Contrato actualizar para ciudades; no expone navegaciones.</summary>
public sealed class ActualizarCiudadDto
{
    [JsonRequired]
    public int DepartamentoId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [MaxLength(64)]
    public string? Codigo { get; set; }
    [JsonRequired]
    public decimal Latitud { get; set; }
    [JsonRequired]
    public decimal Longitud { get; set; }
}

/// <summary>Contrato respuesta para ciudades; no expone navegaciones.</summary>
public sealed class RespuestaCiudadDto
{
    public int Id { get; set; }
    public int DepartamentoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Codigo { get; set; }
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
}

