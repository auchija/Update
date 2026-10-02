using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para categorias; no expone navegaciones.</summary>
public sealed class CrearCategoriaDto
{
    public int? PadreId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Slug { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [MaxLength(255)]
    public string? Icono { get; set; }
    public short Orden { get; set; } = (short)0;
    public bool Activo { get; set; } = true;
}

/// <summary>Contrato actualizar para categorias; no expone navegaciones.</summary>
public sealed class ActualizarCategoriaDto
{
    public int? PadreId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Slug { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [MaxLength(255)]
    public string? Icono { get; set; }
    public short Orden { get; set; } = (short)0;
    public bool Activo { get; set; } = true;
}

/// <summary>Contrato respuesta para categorias; no expone navegaciones.</summary>
public sealed class RespuestaCategoriaDto
{
    public int Id { get; set; }
    public int? PadreId { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Icono { get; set; }
    public short Orden { get; set; } = (short)0;
    public bool Activo { get; set; } = true;
}

