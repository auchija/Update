using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para participantes; no expone navegaciones.</summary>
public sealed class CrearParticipanteDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid ConversacionId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilId { get; set; }
    [EnumDataType(typeof(RolParticipante))]
    public RolParticipante Rol { get; set; } = RolParticipante.Miembro;
    [GuidNoVacio]
    public Guid? UsuarioAsignadoId { get; set; }
    [FechaUtc]
    public DateTime? SalioEn { get; set; }
    [GuidNoVacio]
    public Guid? UltimoMensajeLeidoId { get; set; }
    [FechaUtc]
    public DateTime? UltimaLecturaEn { get; set; }
    [FechaUtc]
    public DateTime? SilenciadoHasta { get; set; }
    [FechaUtc]
    public DateTime? ArchivadoEn { get; set; }
}

/// <summary>Contrato actualizar para participantes; no expone navegaciones.</summary>
public sealed class ActualizarParticipanteDto
{
    [EnumDataType(typeof(RolParticipante))]
    public RolParticipante Rol { get; set; } = RolParticipante.Miembro;
    [GuidNoVacio]
    public Guid? UsuarioAsignadoId { get; set; }
    [FechaUtc]
    public DateTime? SalioEn { get; set; }
    [GuidNoVacio]
    public Guid? UltimoMensajeLeidoId { get; set; }
    [FechaUtc]
    public DateTime? UltimaLecturaEn { get; set; }
    [FechaUtc]
    public DateTime? SilenciadoHasta { get; set; }
    [FechaUtc]
    public DateTime? ArchivadoEn { get; set; }
}

/// <summary>Contrato respuesta para participantes; no expone navegaciones.</summary>
public sealed class RespuestaParticipanteDto
{
    public Guid ConversacionId { get; set; }
    public Guid PerfilId { get; set; }
    public RolParticipante Rol { get; set; } = RolParticipante.Miembro;
    public Guid? UsuarioAsignadoId { get; set; }
    public DateTime UnidoEn { get; set; } = DateTime.UtcNow;
    public DateTime? SalioEn { get; set; }
    public Guid? UltimoMensajeLeidoId { get; set; }
    public DateTime? UltimaLecturaEn { get; set; }
    public DateTime? SilenciadoHasta { get; set; }
    public DateTime? ArchivadoEn { get; set; }
}

