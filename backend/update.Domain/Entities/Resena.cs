using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla resenas.</summary>
public sealed class Resena : BaseEntity, IEntidadValidable, IEliminable
{
    /// <summary>Columna usuario_resenador_id (uuid); obligatoria.</summary>
    public Guid UsuarioResenadorId { get; set; }
    /// <summary>Columna emprendimiento_id (uuid); obligatoria.</summary>
    public Guid EmprendimientoId { get; set; }
    /// <summary>Columna producto_id (uuid); opcional.</summary>
    public Guid? ProductoId { get; set; }
    /// <summary>Columna solicitud_cotizacion_id (uuid); opcional.</summary>
    public Guid? SolicitudCotizacionId { get; set; }
    /// <summary>Columna calificacion (smallint); obligatoria.</summary>
    public short Calificacion { get; set; }
    /// <summary>Columna titulo (text); opcional.</summary>
    public string? Titulo { get; set; }
    /// <summary>Columna contenido (text); opcional.</summary>
    public string? Contenido { get; set; }
    /// <summary>Columna estado (estado_resena); obligatoria.</summary>
    public EstadoResena Estado { get; set; } = EstadoResena.Publicado;
    /// <summary>Columna respuesta_emprendimiento (text); opcional.</summary>
    public string? RespuestaEmprendimiento { get; set; }
    /// <summary>Columna respondido_por_usuario_id (uuid); opcional.</summary>
    public Guid? RespondidoPorUsuarioId { get; set; }
    /// <summary>Columna respondido_en (timestamptz); opcional.</summary>
    public DateTime? RespondidoEn { get; set; }
    /// <summary>Columna eliminado_en (timestamptz); opcional.</summary>
    public DateTime? EliminadoEn { get; set; }
    /// <summary>Relación hacia usuarios mediante usuario_resenador_id.</summary>
    [JsonIgnore]
    public Usuario UsuarioResenador { get; set; } = null!;
    /// <summary>Relación hacia emprendimientos mediante emprendimiento_id.</summary>
    [JsonIgnore]
    public Emprendimiento Emprendimiento { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante respondido_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario? RespondidoPorUsuario { get; set; }
    /// <summary>Relación hacia productos mediante producto_id, emprendimiento_id.</summary>
    [JsonIgnore]
    public Producto? Producto { get; set; }
    /// <summary>Relación hacia solicitudes_cotizacion mediante solicitud_cotizacion_id.</summary>
    [JsonIgnore]
    public SolicitudCotizacion? SolicitudCotizacion { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(UsuarioResenadorId != Guid.Empty, "UsuarioResenadorId es obligatorio.");
        Reglas.Exigir(EmprendimientoId != Guid.Empty, "EmprendimientoId es obligatorio.");
        Reglas.Exigir(Calificacion is >= 1 and <= 5, "Calificacion debe estar entre 1 y 5.");
        Reglas.Texto(Titulo, nameof(Titulo), 255, false, false);
        Reglas.Texto(Contenido, nameof(Contenido), 10000, false, false);
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.Texto(RespuestaEmprendimiento, nameof(RespuestaEmprendimiento), 10000, false, false);
        Reglas.FechaUtc(RespondidoEn, nameof(RespondidoEn));
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
        Reglas.FechaUtc(EliminadoEn, nameof(EliminadoEn));
    }

    /// <summary>Marca el registro como eliminado sin borrarlo físicamente.</summary>
    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }
}
