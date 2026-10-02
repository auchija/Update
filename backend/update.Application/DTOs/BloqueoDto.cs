using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para bloqueos; no expone navegaciones.</summary>
public sealed class CrearBloqueoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioBloqueadorId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilBloqueadoId { get; set; }
}

/// <summary>Contrato actualizar para bloqueos; no expone navegaciones.</summary>
public sealed class ActualizarBloqueoDto
{
}

/// <summary>Contrato respuesta para bloqueos; no expone navegaciones.</summary>
public sealed class RespuestaBloqueoDto
{
    public Guid UsuarioBloqueadorId { get; set; }
    public Guid PerfilBloqueadoId { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
}

