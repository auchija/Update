using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para cotizaciones; no expone navegaciones.</summary>
public sealed class CrearCotizacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid SolicitudCotizacionId { get; set; }
    [JsonRequired]
    public short Version { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(3)]
    [MinLength(3)]
    public string MonedaCodigo { get; set; } = "COP";
    public decimal Subtotal { get; set; } = 0m;
    public decimal Descuento { get; set; } = 0m;
    public decimal Impuesto { get; set; } = 0m;
    public DateOnly? ValidoHasta { get; set; }
    [MaxLength(4000)]
    public string? Notas { get; set; }
    [EnumDataType(typeof(EstadoCotizacion))]
    public EstadoCotizacion Estado { get; set; } = EstadoCotizacion.Enviado;
}

/// <summary>Contrato actualizar para cotizaciones; no expone navegaciones.</summary>
public sealed class ActualizarCotizacionDto
{
    [JsonRequired]
    public short Version { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(3)]
    [MinLength(3)]
    public string MonedaCodigo { get; set; } = "COP";
    public decimal Subtotal { get; set; } = 0m;
    public decimal Descuento { get; set; } = 0m;
    public decimal Impuesto { get; set; } = 0m;
    public DateOnly? ValidoHasta { get; set; }
    [MaxLength(4000)]
    public string? Notas { get; set; }
    [EnumDataType(typeof(EstadoCotizacion))]
    public EstadoCotizacion Estado { get; set; } = EstadoCotizacion.Enviado;
}

/// <summary>Contrato respuesta para cotizaciones; no expone navegaciones.</summary>
public sealed class RespuestaCotizacionDto
{
    public Guid Id { get; set; }
    public Guid SolicitudCotizacionId { get; set; }
    public short Version { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public string MonedaCodigo { get; set; } = "COP";
    public decimal Subtotal { get; set; } = 0m;
    public decimal Descuento { get; set; } = 0m;
    public decimal Impuesto { get; set; } = 0m;
    public decimal? Total { get; set; }
    public DateOnly? ValidoHasta { get; set; }
    public string? Notas { get; set; }
    public EstadoCotizacion Estado { get; set; } = EstadoCotizacion.Enviado;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
}

