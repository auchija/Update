using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla solicitudes_cotizacion.</summary>
public sealed class SolicitudCotizacion : BaseEntity, IEntidadValidable
{
    /// <summary>Columna numero (bigint); obligatoria.</summary>
    public long Numero { get; set; }
    /// <summary>Columna emprendimiento_id (uuid); obligatoria.</summary>
    public Guid EmprendimientoId { get; set; }
    /// <summary>Columna perfil_solicitante_id (uuid); obligatoria.</summary>
    public Guid PerfilSolicitanteId { get; set; }
    /// <summary>Columna creado_por_usuario_id (uuid); obligatoria.</summary>
    public Guid CreadoPorUsuarioId { get; set; }
    /// <summary>Columna conversacion_id (uuid); opcional.</summary>
    public Guid? ConversacionId { get; set; }
    /// <summary>Columna titulo (text); obligatoria.</summary>
    public string Titulo { get; set; } = string.Empty;
    /// <summary>Columna mensaje (text); opcional.</summary>
    public string? Mensaje { get; set; }
    /// <summary>Columna fecha_deseada (date); opcional.</summary>
    public DateOnly? FechaDeseada { get; set; }
    /// <summary>Columna ciudad_entrega_id (integer); opcional.</summary>
    public int? CiudadEntregaId { get; set; }
    /// <summary>Columna estado (estado_solicitud); obligatoria.</summary>
    public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;
    /// <summary>Columna cotizacion_aceptada_id (uuid); opcional.</summary>
    public Guid? CotizacionAceptadaId { get; set; }
    /// <summary>Columna cerrado_en (timestamptz); opcional.</summary>
    public DateTime? CerradoEn { get; set; }
    /// <summary>Relación hacia emprendimientos mediante emprendimiento_id.</summary>
    [JsonIgnore]
    public Emprendimiento Emprendimiento { get; set; } = null!;
    /// <summary>Relación hacia perfiles mediante perfil_solicitante_id.</summary>
    [JsonIgnore]
    public Perfil PerfilSolicitante { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante creado_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    /// <summary>Relación hacia conversaciones mediante conversacion_id.</summary>
    [JsonIgnore]
    public Conversacion? Conversacion { get; set; }
    /// <summary>Relación hacia ciudades mediante ciudad_entrega_id.</summary>
    [JsonIgnore]
    public Ciudad? CiudadEntrega { get; set; }
    /// <summary>Relación hacia cotizaciones mediante cotizacion_aceptada_id, id.</summary>
    [JsonIgnore]
    public Cotizacion? CotizacionAceptada { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(EmprendimientoId != Guid.Empty, "EmprendimientoId es obligatorio.");
        Reglas.Exigir(PerfilSolicitanteId != Guid.Empty, "PerfilSolicitanteId es obligatorio.");
        Reglas.Exigir(CreadoPorUsuarioId != Guid.Empty, "CreadoPorUsuarioId es obligatorio.");
        Reglas.Texto(Titulo, nameof(Titulo), 255, true, false);
        Reglas.Texto(Mensaje, nameof(Mensaje), 10000, false, false);
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
        Reglas.FechaUtc(CerradoEn, nameof(CerradoEn));
    }
}
