using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para productos_etiquetas; no expone navegaciones.</summary>
public sealed class CrearProductoEtiquetaDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid ProductoId { get; set; }
    [JsonRequired]
    public int EtiquetaId { get; set; }
}

/// <summary>Contrato actualizar para productos_etiquetas; no expone navegaciones.</summary>
public sealed class ActualizarProductoEtiquetaDto
{
}

/// <summary>Contrato respuesta para productos_etiquetas; no expone navegaciones.</summary>
public sealed class RespuestaProductoEtiquetaDto
{
    public Guid ProductoId { get; set; }
    public int EtiquetaId { get; set; }
}

