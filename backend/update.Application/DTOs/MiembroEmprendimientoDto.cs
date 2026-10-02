using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para miembros_emprendimiento; no expone navegaciones.</summary>
public sealed class CrearMiembroEmprendimientoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid EmprendimientoId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid UsuarioId { get; set; }
    [EnumDataType(typeof(RolEmprendimiento))]
    public RolEmprendimiento Rol { get; set; } = RolEmprendimiento.Miembro;
    [MaxLength(255)]
    public string? Titulo { get; set; }
    [EnumDataType(typeof(EstadoMiembro))]
    public EstadoMiembro Estado { get; set; } = EstadoMiembro.Invitado;
    [GuidNoVacio]
    public Guid? InvitadoPorUsuarioId { get; set; }
    [FechaUtc]
    public DateTime? UnidoEn { get; set; }
}

/// <summary>Contrato actualizar para miembros_emprendimiento; no expone navegaciones.</summary>
public sealed class ActualizarMiembroEmprendimientoDto
{
    [EnumDataType(typeof(RolEmprendimiento))]
    public RolEmprendimiento Rol { get; set; } = RolEmprendimiento.Miembro;
    [MaxLength(255)]
    public string? Titulo { get; set; }
    [EnumDataType(typeof(EstadoMiembro))]
    public EstadoMiembro Estado { get; set; } = EstadoMiembro.Invitado;
    [GuidNoVacio]
    public Guid? InvitadoPorUsuarioId { get; set; }
    [FechaUtc]
    public DateTime? UnidoEn { get; set; }
}

/// <summary>Contrato respuesta para miembros_emprendimiento; no expone navegaciones.</summary>
public sealed class RespuestaMiembroEmprendimientoDto
{
    public Guid EmprendimientoId { get; set; }
    public Guid UsuarioId { get; set; }
    public RolEmprendimiento Rol { get; set; } = RolEmprendimiento.Miembro;
    public string? Titulo { get; set; }
    public EstadoMiembro Estado { get; set; } = EstadoMiembro.Invitado;
    public Guid? InvitadoPorUsuarioId { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? UnidoEn { get; set; }
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
}

