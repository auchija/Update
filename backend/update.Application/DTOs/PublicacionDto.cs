using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Application.Validation;
namespace update.Application.DTOs;

/// <summary>Contrato crear para publicaciones; no expone navegaciones.</summary>
public sealed class CrearPublicacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilAutorId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string TipoPublicacionCodigo { get; set; } = "general";
    [Required(AllowEmptyStrings = true)]
    [MaxLength(10000)]
    public string Contenido { get; set; } = "";
    [EnumDataType(typeof(VisibilidadPublicacion))]
    public VisibilidadPublicacion Visibilidad { get; set; } = VisibilidadPublicacion.Publico;
    [GuidNoVacio]
    public Guid? ProductoId { get; set; }
    [GuidNoVacio]
    public Guid? OportunidadId { get; set; }
    [GuidNoVacio]
    public Guid? PublicacionCompartidaId { get; set; }
    [GuidNoVacio]
    public Guid? ComentarioAceptadoId { get; set; }
    public bool ComentariosHabilitados { get; set; } = true;
    public bool Fijado { get; set; } = false;
    public int TotalReacciones { get; set; } = 0;
    public int TotalComentarios { get; set; } = 0;
    public int TotalCompartidos { get; set; } = 0;
    public int TotalGuardados { get; set; } = 0;
    public double PuntajeDescubrimiento { get; set; } = 0d;
    [FechaUtc]
    public DateTime? PuntajeActualizadoEn { get; set; }
}

/// <summary>Contrato actualizar para publicaciones; no expone navegaciones.</summary>
public sealed class ActualizarPublicacionDto
{
    [JsonRequired]
    [GuidNoVacio]
    public Guid PerfilAutorId { get; set; }
    [JsonRequired]
    [GuidNoVacio]
    public Guid CreadoPorUsuarioId { get; set; }
    [Required(AllowEmptyStrings = false)]
    [MaxLength(64)]
    public string TipoPublicacionCodigo { get; set; } = "general";
    [Required(AllowEmptyStrings = true)]
    [MaxLength(10000)]
    public string Contenido { get; set; } = "";
    [EnumDataType(typeof(VisibilidadPublicacion))]
    public VisibilidadPublicacion Visibilidad { get; set; } = VisibilidadPublicacion.Publico;
    [GuidNoVacio]
    public Guid? ProductoId { get; set; }
    [GuidNoVacio]
    public Guid? OportunidadId { get; set; }
    [GuidNoVacio]
    public Guid? PublicacionCompartidaId { get; set; }
    [GuidNoVacio]
    public Guid? ComentarioAceptadoId { get; set; }
    public bool ComentariosHabilitados { get; set; } = true;
    public bool Fijado { get; set; } = false;
    public int TotalReacciones { get; set; } = 0;
    public int TotalComentarios { get; set; } = 0;
    public int TotalCompartidos { get; set; } = 0;
    public int TotalGuardados { get; set; } = 0;
    public double PuntajeDescubrimiento { get; set; } = 0d;
    [FechaUtc]
    public DateTime? PuntajeActualizadoEn { get; set; }
}

/// <summary>Contrato respuesta para publicaciones; no expone navegaciones.</summary>
public sealed class RespuestaPublicacionDto
{
    public Guid Id { get; set; }
    public Guid PerfilAutorId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public string TipoPublicacionCodigo { get; set; } = "general";
    public string Contenido { get; set; } = "";
    public VisibilidadPublicacion Visibilidad { get; set; } = VisibilidadPublicacion.Publico;
    public Guid? ProductoId { get; set; }
    public Guid? OportunidadId { get; set; }
    public Guid? PublicacionCompartidaId { get; set; }
    public Guid? ComentarioAceptadoId { get; set; }
    public bool ComentariosHabilitados { get; set; } = true;
    public bool Fijado { get; set; } = false;
    public int TotalReacciones { get; set; } = 0;
    public int TotalComentarios { get; set; } = 0;
    public int TotalCompartidos { get; set; } = 0;
    public int TotalGuardados { get; set; } = 0;
    public double PuntajeDescubrimiento { get; set; } = 0d;
    public DateTime? PuntajeActualizadoEn { get; set; }
    public DateTime? EditadoEn { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? EliminadoEn { get; set; }
}

