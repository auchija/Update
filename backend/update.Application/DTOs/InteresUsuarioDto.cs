using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para intereses_usuario; no expone navegaciones.</summary>
public sealed class CrearInteresUsuarioDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioId { get; set; }
    [JsonRequired]
    public int CategoriaId { get; set; }
}

/// <summary>Contrato actualizar para intereses_usuario; no expone navegaciones.</summary>
public sealed class ActualizarInteresUsuarioDto
{
}

/// <summary>Contrato respuesta para intereses_usuario; no expone navegaciones.</summary>
public sealed class RespuestaInteresUsuarioDto
{
    public Guid UsuarioId { get; set; }
    public int CategoriaId { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
}

