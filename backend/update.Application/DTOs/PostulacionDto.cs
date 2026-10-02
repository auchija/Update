using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para postulaciones; no expone navegaciones.</summary>
public sealed class CrearPostulacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid OportunidadId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilPostulanteId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Mensaje { get; set; } = string.Empty;
    [EnumDataType(typeof(EstadoPostulacion))]
    public EstadoPostulacion Estado { get; set; } = EstadoPostulacion.Pendiente;
}

/// <summary>Contrato actualizar para postulaciones; no expone navegaciones.</summary>
public sealed class ActualizarPostulacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid OportunidadId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilPostulanteId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(10000)]
    public string Mensaje { get; set; } = string.Empty;
    [EnumDataType(typeof(EstadoPostulacion))]
    public EstadoPostulacion Estado { get; set; } = EstadoPostulacion.Pendiente;
}

/// <summary>Contrato respuesta para postulaciones; no expone navegaciones.</summary>
public sealed class RespuestaPostulacionDto
{
    public Guid Id { get; set; }
    public Guid OportunidadId { get; set; }
    public Guid PerfilPostulanteId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public EstadoPostulacion Estado { get; set; } = EstadoPostulacion.Pendiente;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
}

