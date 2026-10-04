using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para productos; no expone navegaciones.</summary>
public sealed class CrearProductoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid EmprendimientoId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [JsonRequired]
    [EnumDataType(typeof(TipoProducto))]
    public TipoProducto Tipo { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Titulo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Slug { get; set; } = string.Empty;
    [MaxLength(10000)]
    public string? Descripcion { get; set; }
    [JsonRequired]
    public int CategoriaId { get; set; }
    [EnumDataType(typeof(TipoPrecio))]
    public TipoPrecio TipoPrecio { get; set; } = TipoPrecio.Fijo;
    public decimal? Precio { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(3)]
    [MinLength(3)]
    public string MonedaCodigo { get; set; } = "COP";
    [EnumDataType(typeof(EstadoInventario))]
    public EstadoInventario EstadoInventario { get; set; } = EstadoInventario.Disponible;
    public int? CantidadInventario { get; set; }
    public int? CiudadId { get; set; }
    [EnumDataType(typeof(EstadoProducto))]
    public EstadoProducto Estado { get; set; } = EstadoProducto.Borrador;
    public decimal? CalificacionPromedio { get; set; }
    public int TotalCalificaciones { get; set; } = 0;
    public int TotalGuardados { get; set; } = 0;
    [FechaUtc]
    public DateTime? PublicadoEn { get; set; }
}

/// <summary>Contrato actualizar para productos; no expone navegaciones.</summary>
public sealed class ActualizarProductoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [JsonRequired]
    [EnumDataType(typeof(TipoProducto))]
    public TipoProducto Tipo { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Titulo { get; set; } = string.Empty;
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(255)]
    public string Slug { get; set; } = string.Empty;
    [MaxLength(10000)]
    public string? Descripcion { get; set; }
    [JsonRequired]
    public int CategoriaId { get; set; }
    [EnumDataType(typeof(TipoPrecio))]
    public TipoPrecio TipoPrecio { get; set; } = TipoPrecio.Fijo;
    public decimal? Precio { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(3)]
    [MinLength(3)]
    public string MonedaCodigo { get; set; } = "COP";
    [EnumDataType(typeof(EstadoInventario))]
    public EstadoInventario EstadoInventario { get; set; } = EstadoInventario.Disponible;
    public int? CantidadInventario { get; set; }
    public int? CiudadId { get; set; }
    [EnumDataType(typeof(EstadoProducto))]
    public EstadoProducto Estado { get; set; } = EstadoProducto.Borrador;
    public decimal? CalificacionPromedio { get; set; }
    public int TotalCalificaciones { get; set; } = 0;
    public int TotalGuardados { get; set; } = 0;
    [FechaUtc]
    public DateTime? PublicadoEn { get; set; }
}

/// <summary>Contrato respuesta para productos; no expone navegaciones.</summary>
public sealed class RespuestaProductoDto
{
    public Guid Id { get; set; }
    public Guid EmprendimientoId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public TipoProducto Tipo { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int CategoriaId { get; set; }
    public TipoPrecio TipoPrecio { get; set; } = TipoPrecio.Fijo;
    public decimal? Precio { get; set; }
    public string MonedaCodigo { get; set; } = "COP";
    public EstadoInventario EstadoInventario { get; set; } = EstadoInventario.Disponible;
    public int? CantidadInventario { get; set; }
    public int? CiudadId { get; set; }
    public EstadoProducto Estado { get; set; } = EstadoProducto.Borrador;
    public decimal? CalificacionPromedio { get; set; }
    public int TotalCalificaciones { get; set; } = 0;
    public int TotalGuardados { get; set; } = 0;
    public DateTime? PublicadoEn { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? EliminadoEn { get; set; }
}

