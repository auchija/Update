using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para resenas_archivos; no expone navegaciones.</summary>
public sealed class CrearResenaArchivoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid ResenaId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid ArchivoId { get; set; }
    [JsonRequired]
    public short Posicion { get; set; }
}

/// <summary>Contrato actualizar para resenas_archivos; no expone navegaciones.</summary>
public sealed class ActualizarResenaArchivoDto
{
    [JsonRequired]
    public short Posicion { get; set; }
}

/// <summary>Contrato respuesta para resenas_archivos; no expone navegaciones.</summary>
public sealed class RespuestaResenaArchivoDto
{
    public Guid ResenaId { get; set; }
    public Guid ArchivoId { get; set; }
    public short Posicion { get; set; }
}

