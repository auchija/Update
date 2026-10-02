using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para reacciones_comentario; no expone navegaciones.</summary>
public sealed class CrearReaccionComentarioDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid ComentarioId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string TipoReaccionCodigo { get; set; } = string.Empty;
}

/// <summary>Contrato actualizar para reacciones_comentario; no expone navegaciones.</summary>
public sealed class ActualizarReaccionComentarioDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string TipoReaccionCodigo { get; set; } = string.Empty;
}

/// <summary>Contrato respuesta para reacciones_comentario; no expone navegaciones.</summary>
public sealed class RespuestaReaccionComentarioDto
{
    public Guid ComentarioId { get; set; }
    public Guid PerfilId { get; set; }
    public string TipoReaccionCodigo { get; set; } = string.Empty;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
}

