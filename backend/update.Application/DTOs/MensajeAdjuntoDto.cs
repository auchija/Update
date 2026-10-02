using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para mensajes_adjuntos; no expone navegaciones.</summary>
public sealed class CrearMensajeAdjuntoDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid MensajeId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid ArchivoId { get; set; }
    [JsonRequired]
    public short Posicion { get; set; }
}

/// <summary>Contrato actualizar para mensajes_adjuntos; no expone navegaciones.</summary>
public sealed class ActualizarMensajeAdjuntoDto
{
    [JsonRequired]
    public short Posicion { get; set; }
}

/// <summary>Contrato respuesta para mensajes_adjuntos; no expone navegaciones.</summary>
public sealed class RespuestaMensajeAdjuntoDto
{
    public Guid MensajeId { get; set; }
    public Guid ArchivoId { get; set; }
    public short Posicion { get; set; }
}

