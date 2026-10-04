using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para menciones; no expone navegaciones.</summary>
public sealed class CrearMencionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid PublicacionId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilId { get; set; }
}

/// <summary>Contrato actualizar para menciones; no expone navegaciones.</summary>
public sealed class ActualizarMencionDto
{
}

/// <summary>Contrato respuesta para menciones; no expone navegaciones.</summary>
public sealed class RespuestaMencionDto
{
    public Guid PublicacionId { get; set; }
    public Guid PerfilId { get; set; }
}

