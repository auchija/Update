using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para solicitud_items; no expone navegaciones.</summary>
public sealed class CrearSolicitudItemDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid SolicitudCotizacionId { get; set; }
    [GuidNoVacio]
    public Guid? ProductoId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; } = 1m;
    [MaxLength(4000)]
    public string? Notas { get; set; }
}

/// <summary>Contrato actualizar para solicitud_items; no expone navegaciones.</summary>
public sealed class ActualizarSolicitudItemDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid SolicitudCotizacionId { get; set; }
    [GuidNoVacio]
    public Guid? ProductoId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; } = 1m;
    [MaxLength(4000)]
    public string? Notas { get; set; }
}

/// <summary>Contrato respuesta para solicitud_items; no expone navegaciones.</summary>
public sealed class RespuestaSolicitudItemDto
{
    public Guid Id { get; set; }
    public Guid SolicitudCotizacionId { get; set; }
    public Guid? ProductoId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; } = 1m;
    public string? Notas { get; set; }
}

