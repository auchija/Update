using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para permisos_rol; no expone navegaciones.</summary>
public sealed class CrearPermisoRolDto
{
    [JsonRequired]
    [EnumDataType(typeof(RolEmprendimiento))]
    public RolEmprendimiento Rol { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string Permiso { get; set; } = string.Empty;
}

/// <summary>Contrato actualizar para permisos_rol; no expone navegaciones.</summary>
public sealed class ActualizarPermisoRolDto
{
}

/// <summary>Contrato respuesta para permisos_rol; no expone navegaciones.</summary>
public sealed class RespuestaPermisoRolDto
{
    public RolEmprendimiento Rol { get; set; }
    public string Permiso { get; set; } = string.Empty;
}

