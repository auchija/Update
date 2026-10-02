using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para productos_archivos; no expone navegaciones.</summary>
public sealed class CrearProductoArchivoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid ProductoId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid ArchivoId { get; set; }
    [JsonRequired]
    public short Posicion { get; set; }
    [MaxLength(255)]
    public string? TextoAlternativo { get; set; }
}

/// <summary>Contrato actualizar para productos_archivos; no expone navegaciones.</summary>
public sealed class ActualizarProductoArchivoDto
{
    [JsonRequired]
    public short Posicion { get; set; }
    [MaxLength(255)]
    public string? TextoAlternativo { get; set; }
}

/// <summary>Contrato respuesta para productos_archivos; no expone navegaciones.</summary>
public sealed class RespuestaProductoArchivoDto
{
    public Guid ProductoId { get; set; }
    public Guid ArchivoId { get; set; }
    public short Posicion { get; set; }
    public string? TextoAlternativo { get; set; }
}

