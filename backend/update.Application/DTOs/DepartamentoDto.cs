using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para departamentos; no expone navegaciones.</summary>
public sealed class CrearDepartamentoDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(2)]
    [MinLength(2)]
    public string PaisCodigo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [MaxLength(64)]
    public string? Codigo { get; set; }
}

/// <summary>Contrato actualizar para departamentos; no expone navegaciones.</summary>
public sealed class ActualizarDepartamentoDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(2)]
    [MinLength(2)]
    public string PaisCodigo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [MaxLength(64)]
    public string? Codigo { get; set; }
}

/// <summary>Contrato respuesta para departamentos; no expone navegaciones.</summary>
public sealed class RespuestaDepartamentoDto
{
    public int Id { get; set; }
    public string PaisCodigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Codigo { get; set; }
}

