using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para tipos_persona; no expone navegaciones.</summary>
public sealed class CrearTipoPersonaDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string Codigo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [MaxLength(10000)]
    public string? Descripcion { get; set; }
    public short Orden { get; set; } = (short)0;
    public bool Activo { get; set; } = true;
}

/// <summary>Contrato actualizar para tipos_persona; no expone navegaciones.</summary>
public sealed class ActualizarTipoPersonaDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [MaxLength(10000)]
    public string? Descripcion { get; set; }
    public short Orden { get; set; } = (short)0;
    public bool Activo { get; set; } = true;
}

/// <summary>Contrato respuesta para tipos_persona; no expone navegaciones.</summary>
public sealed class RespuestaTipoPersonaDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public short Orden { get; set; } = (short)0;
    public bool Activo { get; set; } = true;
}

