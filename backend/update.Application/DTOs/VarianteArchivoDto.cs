using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para variantes_archivo; no expone navegaciones.</summary>
public sealed class CrearVarianteArchivoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid ArchivoId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string Variante { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(1024)]
    public string ClaveObjeto { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(127)]
    public string TipoMime { get; set; } = string.Empty;
    public int? Ancho { get; set; }
    public int? Alto { get; set; }
    [JsonRequired]
    public long TamanoBytes { get; set; }
}

/// <summary>Contrato actualizar para variantes_archivo; no expone navegaciones.</summary>
public sealed class ActualizarVarianteArchivoDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(1024)]
    public string ClaveObjeto { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(127)]
    public string TipoMime { get; set; } = string.Empty;
    public int? Ancho { get; set; }
    public int? Alto { get; set; }
    [JsonRequired]
    public long TamanoBytes { get; set; }
}

/// <summary>Contrato respuesta para variantes_archivo; no expone navegaciones.</summary>
public sealed class RespuestaVarianteArchivoDto
{
    public Guid ArchivoId { get; set; }
    public string Variante { get; set; } = string.Empty;
    public string ClaveObjeto { get; set; } = string.Empty;
    public string TipoMime { get; set; } = string.Empty;
    public int? Ancho { get; set; }
    public int? Alto { get; set; }
    public long TamanoBytes { get; set; }
}

