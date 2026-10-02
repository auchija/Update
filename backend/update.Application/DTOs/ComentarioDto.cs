using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para comentarios; no expone navegaciones.</summary>
public sealed class CrearComentarioDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid PublicacionId { get; set; }
    [GuidNoVacio]
    public Guid? ComentarioPadreId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilAutorId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [GuidNoVacio]
    public Guid? RespondeAPerfilId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Contenido { get; set; } = string.Empty;
    public int TotalReacciones { get; set; } = 0;
    public int TotalRespuestas { get; set; } = 0;
}

/// <summary>Contrato actualizar para comentarios; no expone navegaciones.</summary>
public sealed class ActualizarComentarioDto
{
    [GuidNoVacio]
    public Guid? ComentarioPadreId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilAutorId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [GuidNoVacio]
    public Guid? RespondeAPerfilId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Contenido { get; set; } = string.Empty;
    public int TotalReacciones { get; set; } = 0;
    public int TotalRespuestas { get; set; } = 0;
}

/// <summary>Contrato respuesta para comentarios; no expone navegaciones.</summary>
public sealed class RespuestaComentarioDto
{
    public Guid Id { get; set; }
    public Guid PublicacionId { get; set; }
    public Guid? ComentarioPadreId { get; set; }
    public Guid PerfilAutorId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public Guid? RespondeAPerfilId { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public int TotalReacciones { get; set; } = 0;
    public int TotalRespuestas { get; set; } = 0;
    public DateTime? EditadoEn { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? EliminadoEn { get; set; }
}

