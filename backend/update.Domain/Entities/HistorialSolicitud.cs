using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla historial_solicitudes.</summary>
public sealed class HistorialSolicitud : IEntidadValidable
{
    /// <summary>Columna id (bigint); obligatoria.</summary>
    public long Id { get; set; }
    /// <summary>Columna solicitud_cotizacion_id (uuid); obligatoria.</summary>
    public Guid SolicitudCotizacionId { get; set; }
    /// <summary>Columna estado_anterior (estado_solicitud); opcional.</summary>
    public EstadoSolicitud? EstadoAnterior { get; set; }
    /// <summary>Columna estado_nuevo (estado_solicitud); obligatoria.</summary>
    public EstadoSolicitud EstadoNuevo { get; set; }
    /// <summary>Columna usuario_actor_id (uuid); opcional.</summary>
    public Guid? UsuarioActorId { get; set; }
    /// <summary>Columna nota (text); opcional.</summary>
    public string? Nota { get; set; }
    /// <summary>Columna creado_en (timestamptz); obligatoria.</summary>
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    /// <summary>Relación hacia solicitudes_cotizacion mediante solicitud_cotizacion_id.</summary>
    [JsonIgnore]
    public SolicitudCotizacion SolicitudCotizacion { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante usuario_actor_id.</summary>
    [JsonIgnore]
    public Usuario? UsuarioActor { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(SolicitudCotizacionId != Guid.Empty, "SolicitudCotizacionId es obligatorio.");
        Reglas.Exigir(EstadoAnterior is null || Enum.IsDefined(EstadoAnterior.Value), "EstadoAnterior contiene un valor no permitido.");
        Reglas.Exigir(Enum.IsDefined(EstadoNuevo), "EstadoNuevo contiene un valor no permitido.");
        Reglas.Texto(Nota, nameof(Nota), 4000, false, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }
}
