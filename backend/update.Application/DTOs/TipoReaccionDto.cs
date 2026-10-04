using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para tipos_reaccion; no expone navegaciones.</summary>
public sealed class CrearTipoReaccionDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string Codigo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(32)]
    public string Emoji { get; set; } = string.Empty;
    public short Orden { get; set; } = (short)0;
    public bool Activo { get; set; } = true;
}

/// <summary>Contrato actualizar para tipos_reaccion; no expone navegaciones.</summary>
public sealed class ActualizarTipoReaccionDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(32)]
    public string Emoji { get; set; } = string.Empty;
    public short Orden { get; set; } = (short)0;
    public bool Activo { get; set; } = true;
}

/// <summary>Contrato respuesta para tipos_reaccion; no expone navegaciones.</summary>
public sealed class RespuestaTipoReaccionDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;
    public short Orden { get; set; } = (short)0;
    public bool Activo { get; set; } = true;
}

