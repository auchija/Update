using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para etiquetas; no expone navegaciones.</summary>
public sealed class CrearEtiquetaDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    public int TotalUsos { get; set; } = 0;
}

/// <summary>Contrato actualizar para etiquetas; no expone navegaciones.</summary>
public sealed class ActualizarEtiquetaDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    public int TotalUsos { get; set; } = 0;
}

/// <summary>Contrato respuesta para etiquetas; no expone navegaciones.</summary>
public sealed class RespuestaEtiquetaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int TotalUsos { get; set; } = 0;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
}

