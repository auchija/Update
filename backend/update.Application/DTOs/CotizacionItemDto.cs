using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para cotizacion_items; no expone navegaciones.</summary>
public sealed class CrearCotizacionItemDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid CotizacionId { get; set; }
    [GuidNoVacio]
    public Guid? ProductoId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Descripcion { get; set; } = string.Empty;
    [JsonRequired]
    public decimal Cantidad { get; set; }
    [JsonRequired]
    public decimal PrecioUnitario { get; set; }
}

/// <summary>Contrato actualizar para cotizacion_items; no expone navegaciones.</summary>
public sealed class ActualizarCotizacionItemDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid CotizacionId { get; set; }
    [GuidNoVacio]
    public Guid? ProductoId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Descripcion { get; set; } = string.Empty;
    [JsonRequired]
    public decimal Cantidad { get; set; }
    [JsonRequired]
    public decimal PrecioUnitario { get; set; }
}

/// <summary>Contrato respuesta para cotizacion_items; no expone navegaciones.</summary>
public sealed class RespuestaCotizacionItemDto
{
    public Guid Id { get; set; }
    public Guid CotizacionId { get; set; }
    public Guid? ProductoId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal? TotalLinea { get; set; }
}

