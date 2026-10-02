using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para usuario_tipos_persona; no expone navegaciones.</summary>
public sealed class CrearUsuarioTipoPersonaDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string TipoPersonaCodigo { get; set; } = string.Empty;
}

/// <summary>Contrato actualizar para usuario_tipos_persona; no expone navegaciones.</summary>
public sealed class ActualizarUsuarioTipoPersonaDto
{
}

/// <summary>Contrato respuesta para usuario_tipos_persona; no expone navegaciones.</summary>
public sealed class RespuestaUsuarioTipoPersonaDto
{
    public Guid UsuarioId { get; set; }
    public string TipoPersonaCodigo { get; set; } = string.Empty;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
}

