using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para perfiles; no expone navegaciones.</summary>
public sealed class CrearPerfilDto
{
    [JsonRequired]
    [EnumDataType(typeof(TipoPerfil))]
    public TipoPerfil Tipo { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(50)]
    public string NombreUsuario { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string NombreVisible { get; set; } = string.Empty;
    [MaxLength(255)]
    public string? Titular { get; set; }
    [MaxLength(2000)]
    public string? Biografia { get; set; }
    [GuidNoVacio]
    public Guid? FotoArchivoId { get; set; }
    [GuidNoVacio]
    public Guid? PortadaArchivoId { get; set; }
    public int? CiudadId { get; set; }
    [MaxLength(2048)]
    public string? SitioWeb { get; set; }
    public bool EsPrivado { get; set; } = false;
    [EnumDataType(typeof(EstadoCuenta))]
    public EstadoCuenta Estado { get; set; } = EstadoCuenta.Activo;
    public int TotalSeguidores { get; set; } = 0;
    public int TotalSeguidos { get; set; } = 0;
    public int TotalPublicaciones { get; set; } = 0;
}

/// <summary>Contrato actualizar para perfiles; no expone navegaciones.</summary>
public sealed class ActualizarPerfilDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(50)]
    public string NombreUsuario { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string NombreVisible { get; set; } = string.Empty;
    [MaxLength(255)]
    public string? Titular { get; set; }
    [MaxLength(2000)]
    public string? Biografia { get; set; }
    [GuidNoVacio]
    public Guid? FotoArchivoId { get; set; }
    [GuidNoVacio]
    public Guid? PortadaArchivoId { get; set; }
    public int? CiudadId { get; set; }
    [MaxLength(2048)]
    public string? SitioWeb { get; set; }
    public bool EsPrivado { get; set; } = false;
    [EnumDataType(typeof(EstadoCuenta))]
    public EstadoCuenta Estado { get; set; } = EstadoCuenta.Activo;
    public int TotalSeguidores { get; set; } = 0;
    public int TotalSeguidos { get; set; } = 0;
    public int TotalPublicaciones { get; set; } = 0;
}

/// <summary>Contrato respuesta para perfiles; no expone navegaciones.</summary>
public sealed class RespuestaPerfilDto
{
    public Guid Id { get; set; }
    public TipoPerfil Tipo { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string NombreVisible { get; set; } = string.Empty;
    public string? Titular { get; set; }
    public string? Biografia { get; set; }
    public Guid? FotoArchivoId { get; set; }
    public Guid? PortadaArchivoId { get; set; }
    public int? CiudadId { get; set; }
    public string? SitioWeb { get; set; }
    public bool EsPrivado { get; set; } = false;
    public EstadoCuenta Estado { get; set; } = EstadoCuenta.Activo;
    public int TotalSeguidores { get; set; } = 0;
    public int TotalSeguidos { get; set; } = 0;
    public int TotalPublicaciones { get; set; } = 0;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? EliminadoEn { get; set; }
}

