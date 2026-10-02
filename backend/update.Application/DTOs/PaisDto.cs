using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para paises; no expone navegaciones.</summary>
public sealed class CrearPaisDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(2)]
    [MinLength(2)]
    public string Codigo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(3)]
    [MinLength(3)]
    public string MonedaDefectoCodigo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(30)]
    public string PrefijoTelefono { get; set; } = string.Empty;
}

/// <summary>Contrato actualizar para paises; no expone navegaciones.</summary>
public sealed class ActualizarPaisDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(3)]
    [MinLength(3)]
    public string MonedaDefectoCodigo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(30)]
    public string PrefijoTelefono { get; set; } = string.Empty;
}

/// <summary>Contrato respuesta para paises; no expone navegaciones.</summary>
public sealed class RespuestaPaisDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string MonedaDefectoCodigo { get; set; } = string.Empty;
    public string PrefijoTelefono { get; set; } = string.Empty;
}

