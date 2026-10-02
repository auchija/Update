using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para archivos; no expone navegaciones.</summary>
public sealed class CrearArchivoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid SubidoPorUsuarioId { get; set; }
    [JsonRequired]
    [EnumDataType(typeof(TipoArchivo))]
    public TipoArchivo Tipo { get; set; }
    [JsonRequired]
    [EnumDataType(typeof(PropositoArchivo))]
    public PropositoArchivo Proposito { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string ProveedorAlmacenamiento { get; set; } = "r2";
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Bucket { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(1024)]
    public string ClaveObjeto { get; set; } = string.Empty;
    [MaxLength(255)]
    public string? NombreOriginal { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(127)]
    public string TipoMime { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(16)]
    public string Extension { get; set; } = string.Empty;
    [JsonRequired]
    public long TamanoBytes { get; set; }
    [MaxLength(64)]
    [MinLength(64)]
    public string? ChecksumSha256 { get; set; }
    public int? Ancho { get; set; }
    public int? Alto { get; set; }
    public decimal? DuracionSegundos { get; set; }
    [EnumDataType(typeof(VisibilidadArchivo))]
    public VisibilidadArchivo Visibilidad { get; set; } = VisibilidadArchivo.Publico;
    [EnumDataType(typeof(EstadoArchivo))]
    public EstadoArchivo Estado { get; set; } = EstadoArchivo.Pendiente;
}

/// <summary>Contrato actualizar para archivos; no expone navegaciones.</summary>
public sealed class ActualizarArchivoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid SubidoPorUsuarioId { get; set; }
    [JsonRequired]
    [EnumDataType(typeof(TipoArchivo))]
    public TipoArchivo Tipo { get; set; }
    [JsonRequired]
    [EnumDataType(typeof(PropositoArchivo))]
    public PropositoArchivo Proposito { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string ProveedorAlmacenamiento { get; set; } = "r2";
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Bucket { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(1024)]
    public string ClaveObjeto { get; set; } = string.Empty;
    [MaxLength(255)]
    public string? NombreOriginal { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(127)]
    public string TipoMime { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(16)]
    public string Extension { get; set; } = string.Empty;
    [JsonRequired]
    public long TamanoBytes { get; set; }
    [MaxLength(64)]
    [MinLength(64)]
    public string? ChecksumSha256 { get; set; }
    public int? Ancho { get; set; }
    public int? Alto { get; set; }
    public decimal? DuracionSegundos { get; set; }
    [EnumDataType(typeof(VisibilidadArchivo))]
    public VisibilidadArchivo Visibilidad { get; set; } = VisibilidadArchivo.Publico;
    [EnumDataType(typeof(EstadoArchivo))]
    public EstadoArchivo Estado { get; set; } = EstadoArchivo.Pendiente;
}

/// <summary>Contrato respuesta para archivos; no expone navegaciones.</summary>
public sealed class RespuestaArchivoDto
{
    public Guid Id { get; set; }
    public Guid SubidoPorUsuarioId { get; set; }
    public TipoArchivo Tipo { get; set; }
    public PropositoArchivo Proposito { get; set; }
    public string ProveedorAlmacenamiento { get; set; } = "r2";
    public string Bucket { get; set; } = string.Empty;
    public string ClaveObjeto { get; set; } = string.Empty;
    public string? NombreOriginal { get; set; }
    public string TipoMime { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long TamanoBytes { get; set; }
    public string? ChecksumSha256 { get; set; }
    public int? Ancho { get; set; }
    public int? Alto { get; set; }
    public decimal? DuracionSegundos { get; set; }
    public VisibilidadArchivo Visibilidad { get; set; } = VisibilidadArchivo.Publico;
    public EstadoArchivo Estado { get; set; } = EstadoArchivo.Pendiente;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? EliminadoEn { get; set; }
}

