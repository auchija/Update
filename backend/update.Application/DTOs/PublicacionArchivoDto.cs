using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para publicaciones_archivos; no expone navegaciones.</summary>
public sealed class CrearPublicacionArchivoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid PublicacionId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid ArchivoId { get; set; }
    [JsonRequired]
    public short Posicion { get; set; }
    [MaxLength(255)]
    public string? TextoAlternativo { get; set; }
}

/// <summary>Contrato actualizar para publicaciones_archivos; no expone navegaciones.</summary>
public sealed class ActualizarPublicacionArchivoDto
{
    [JsonRequired]
    public short Posicion { get; set; }
    [MaxLength(255)]
    public string? TextoAlternativo { get; set; }
}

/// <summary>Contrato respuesta para publicaciones_archivos; no expone navegaciones.</summary>
public sealed class RespuestaPublicacionArchivoDto
{
    public Guid PublicacionId { get; set; }
    public Guid ArchivoId { get; set; }
    public short Posicion { get; set; }
    public string? TextoAlternativo { get; set; }
}

