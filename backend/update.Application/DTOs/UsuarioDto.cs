using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para usuarios; no expone navegaciones.</summary>
public sealed class CrearUsuarioDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid Id { get; set; }
    [EnumDataType(typeof(TipoPerfil))]
    public TipoPerfil TipoPerfil { get; set; } = TipoPerfil.Persona;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(254)]
    [EmailAddress]
    public string Correo { get; set; } = string.Empty;
    [MaxLength(512)]
    public string? HashContrasena { get; set; }
    [EnumDataType(typeof(RolPlataforma))]
    public RolPlataforma RolPlataforma { get; set; } = RolPlataforma.Usuario;
    [FechaUtc]
    public DateTime? CorreoVerificadoEn { get; set; }
    [JsonRequired]
    [FechaUtc]
    public DateTime TerminosAceptadosEn { get; set; }
    [FechaUtc]
    public DateTime? UltimoIngresoEn { get; set; }
    public short IntentosFallidos { get; set; } = (short)0;
    [FechaUtc]
    public DateTime? BloqueadoHasta { get; set; }
    [FechaUtc]
    public DateTime? ContrasenaCambiadaEn { get; set; }
}

/// <summary>Contrato actualizar para usuarios; no expone navegaciones.</summary>
public sealed class ActualizarUsuarioDto
{
    [EnumDataType(typeof(TipoPerfil))]
    public TipoPerfil TipoPerfil { get; set; } = TipoPerfil.Persona;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(254)]
    [EmailAddress]
    public string Correo { get; set; } = string.Empty;
    [MaxLength(512)]
    public string? HashContrasena { get; set; }
    [EnumDataType(typeof(RolPlataforma))]
    public RolPlataforma RolPlataforma { get; set; } = RolPlataforma.Usuario;
    [FechaUtc]
    public DateTime? CorreoVerificadoEn { get; set; }
    [JsonRequired]
    [FechaUtc]
    public DateTime TerminosAceptadosEn { get; set; }
    [FechaUtc]
    public DateTime? UltimoIngresoEn { get; set; }
    public short IntentosFallidos { get; set; } = (short)0;
    [FechaUtc]
    public DateTime? BloqueadoHasta { get; set; }
    [FechaUtc]
    public DateTime? ContrasenaCambiadaEn { get; set; }
}

/// <summary>Contrato respuesta para usuarios; no expone navegaciones.</summary>
public sealed class RespuestaUsuarioDto
{
    public Guid Id { get; set; }
    public TipoPerfil TipoPerfil { get; set; } = TipoPerfil.Persona;
    public string Correo { get; set; } = string.Empty;
    public RolPlataforma RolPlataforma { get; set; } = RolPlataforma.Usuario;
    public DateTime? CorreoVerificadoEn { get; set; }
    public DateTime TerminosAceptadosEn { get; set; }
    public DateTime? UltimoIngresoEn { get; set; }
    public short IntentosFallidos { get; set; } = (short)0;
    public DateTime? BloqueadoHasta { get; set; }
    public DateTime? ContrasenaCambiadaEn { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
}

