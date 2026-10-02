using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para emprendimientos; no expone navegaciones.</summary>
public sealed class CrearEmprendimientoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid Id { get; set; }
    [EnumDataType(typeof(TipoPerfil))]
    public TipoPerfil TipoPerfil { get; set; } = TipoPerfil.Emprendimiento;
    [JsonRequired]
    public int CategoriaId { get; set; }
    [EnumDataType(typeof(EtapaEmprendimiento))]
    public EtapaEmprendimiento Etapa { get; set; } = EtapaEmprendimiento.Idea;
    public DateOnly? FundadoEn { get; set; }
    [MaxLength(255)]
    public string? RazonSocial { get; set; }
    [MaxLength(255)]
    public string? Nit { get; set; }
    [MaxLength(254)]
    [EmailAddress]
    public string? CorreoContacto { get; set; }
    [MaxLength(30)]
    public string? TelefonoContacto { get; set; }
    [MaxLength(30)]
    public string? Whatsapp { get; set; }
    [MaxLength(500)]
    public string? Direccion { get; set; }
    public bool EnviosNacionales { get; set; } = false;
    public bool OfreceRemoto { get; set; } = false;
    [EnumDataType(typeof(EstadoVerificacion))]
    public EstadoVerificacion EstadoVerificacion { get; set; } = EstadoVerificacion.SinVerificar;
    [FechaUtc]
    public DateTime? VerificadoEn { get; set; }
    public decimal? CalificacionPromedio { get; set; }
    public int TotalCalificaciones { get; set; } = 0;
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
}

/// <summary>Contrato actualizar para emprendimientos; no expone navegaciones.</summary>
public sealed class ActualizarEmprendimientoDto
{
    [EnumDataType(typeof(TipoPerfil))]
    public TipoPerfil TipoPerfil { get; set; } = TipoPerfil.Emprendimiento;
    [JsonRequired]
    public int CategoriaId { get; set; }
    [EnumDataType(typeof(EtapaEmprendimiento))]
    public EtapaEmprendimiento Etapa { get; set; } = EtapaEmprendimiento.Idea;
    public DateOnly? FundadoEn { get; set; }
    [MaxLength(255)]
    public string? RazonSocial { get; set; }
    [MaxLength(255)]
    public string? Nit { get; set; }
    [MaxLength(254)]
    [EmailAddress]
    public string? CorreoContacto { get; set; }
    [MaxLength(30)]
    public string? TelefonoContacto { get; set; }
    [MaxLength(30)]
    public string? Whatsapp { get; set; }
    [MaxLength(500)]
    public string? Direccion { get; set; }
    public bool EnviosNacionales { get; set; } = false;
    public bool OfreceRemoto { get; set; } = false;
    [EnumDataType(typeof(EstadoVerificacion))]
    public EstadoVerificacion EstadoVerificacion { get; set; } = EstadoVerificacion.SinVerificar;
    [FechaUtc]
    public DateTime? VerificadoEn { get; set; }
    public decimal? CalificacionPromedio { get; set; }
    public int TotalCalificaciones { get; set; } = 0;
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
}

/// <summary>Contrato respuesta para emprendimientos; no expone navegaciones.</summary>
public sealed class RespuestaEmprendimientoDto
{
    public Guid Id { get; set; }
    public TipoPerfil TipoPerfil { get; set; } = TipoPerfil.Emprendimiento;
    public int CategoriaId { get; set; }
    public EtapaEmprendimiento Etapa { get; set; } = EtapaEmprendimiento.Idea;
    public DateOnly? FundadoEn { get; set; }
    public string? RazonSocial { get; set; }
    public string? Nit { get; set; }
    public string? CorreoContacto { get; set; }
    public string? TelefonoContacto { get; set; }
    public string? Whatsapp { get; set; }
    public string? Direccion { get; set; }
    public bool EnviosNacionales { get; set; } = false;
    public bool OfreceRemoto { get; set; } = false;
    public EstadoVerificacion EstadoVerificacion { get; set; } = EstadoVerificacion.SinVerificar;
    public DateTime? VerificadoEn { get; set; }
    public decimal? CalificacionPromedio { get; set; }
    public int TotalCalificaciones { get; set; } = 0;
    public Guid CreadoPorUsuarioId { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
}

