using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para documentos_verificacion; no expone navegaciones.</summary>
public sealed class CrearDocumentoVerificacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid VerificacionId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid ArchivoId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string TipoDocumento { get; set; } = string.Empty;
}

/// <summary>Contrato actualizar para documentos_verificacion; no expone navegaciones.</summary>
public sealed class ActualizarDocumentoVerificacionDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string TipoDocumento { get; set; } = string.Empty;
}

/// <summary>Contrato respuesta para documentos_verificacion; no expone navegaciones.</summary>
public sealed class RespuestaDocumentoVerificacionDto
{
    public Guid VerificacionId { get; set; }
    public Guid ArchivoId { get; set; }
    public string TipoDocumento { get; set; } = string.Empty;
}

