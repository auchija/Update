using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para reacciones_publicacion; no expone navegaciones.</summary>
public sealed class CrearReaccionPublicacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid PublicacionId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string TipoReaccionCodigo { get; set; } = string.Empty;
}

/// <summary>Contrato actualizar para reacciones_publicacion; no expone navegaciones.</summary>
public sealed class ActualizarReaccionPublicacionDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string TipoReaccionCodigo { get; set; } = string.Empty;
}

/// <summary>Contrato respuesta para reacciones_publicacion; no expone navegaciones.</summary>
public sealed class RespuestaReaccionPublicacionDto
{
    public Guid PublicacionId { get; set; }
    public Guid PerfilId { get; set; }
    public string TipoReaccionCodigo { get; set; } = string.Empty;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
}

