using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para usuarios_reservados; no expone navegaciones.</summary>
public sealed class CrearUsuarioReservadoDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(50)]
    public string NombreUsuario { get; set; } = string.Empty;
}

/// <summary>Contrato actualizar para usuarios_reservados; no expone navegaciones.</summary>
public sealed class ActualizarUsuarioReservadoDto
{
}

/// <summary>Contrato respuesta para usuarios_reservados; no expone navegaciones.</summary>
public sealed class RespuestaUsuarioReservadoDto
{
    public string NombreUsuario { get; set; } = string.Empty;
}

