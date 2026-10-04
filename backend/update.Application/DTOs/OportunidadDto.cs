using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para oportunidades; no expone navegaciones.</summary>
public sealed class CrearOportunidadDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilAutorId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string TipoCodigo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Titulo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Descripcion { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    public int? CiudadId { get; set; }
    public bool EsRemoto { get; set; } = false;
    [EnumDataType(typeof(EstadoOportunidad))]
    public EstadoOportunidad Estado { get; set; } = EstadoOportunidad.Abierto;
    [FechaUtc]
    public DateTime? ExpiraEn { get; set; }
    public int TotalPostulaciones { get; set; } = 0;
}

/// <summary>Contrato actualizar para oportunidades; no expone navegaciones.</summary>
public sealed class ActualizarOportunidadDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilAutorId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string TipoCodigo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Titulo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Descripcion { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    public int? CiudadId { get; set; }
    public bool EsRemoto { get; set; } = false;
    [EnumDataType(typeof(EstadoOportunidad))]
    public EstadoOportunidad Estado { get; set; } = EstadoOportunidad.Abierto;
    [FechaUtc]
    public DateTime? ExpiraEn { get; set; }
    public int TotalPostulaciones { get; set; } = 0;
}

/// <summary>Contrato respuesta para oportunidades; no expone navegaciones.</summary>
public sealed class RespuestaOportunidadDto
{
    public Guid Id { get; set; }
    public Guid PerfilAutorId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public string TipoCodigo { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int? CategoriaId { get; set; }
    public int? CiudadId { get; set; }
    public bool EsRemoto { get; set; } = false;
    public EstadoOportunidad Estado { get; set; } = EstadoOportunidad.Abierto;
    public DateTime? ExpiraEn { get; set; }
    public int TotalPostulaciones { get; set; } = 0;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? EliminadoEn { get; set; }
}

