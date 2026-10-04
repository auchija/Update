using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para publicaciones_etiquetas; no expone navegaciones.</summary>
public sealed class CrearPublicacionEtiquetaDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid PublicacionId { get; set; }
    [JsonRequired]
    public int EtiquetaId { get; set; }
}

/// <summary>Contrato actualizar para publicaciones_etiquetas; no expone navegaciones.</summary>
public sealed class ActualizarPublicacionEtiquetaDto
{
}

/// <summary>Contrato respuesta para publicaciones_etiquetas; no expone navegaciones.</summary>
public sealed class RespuestaPublicacionEtiquetaDto
{
    public Guid PublicacionId { get; set; }
    public int EtiquetaId { get; set; }
}

