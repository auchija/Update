using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla mensajes.</summary>
public sealed class Mensaje : BaseEntity, IEntidadValidable, IEliminable
{
    /// <summary>Columna conversacion_id (uuid); obligatoria.</summary>
    public Guid ConversacionId { get; set; }
    /// <summary>Columna perfil_remitente_id (uuid); obligatoria.</summary>
    public Guid PerfilRemitenteId { get; set; }
    /// <summary>Columna usuario_remitente_id (uuid); opcional.</summary>
    public Guid? UsuarioRemitenteId { get; set; }
    /// <summary>Columna tipo (tipo_mensaje); obligatoria.</summary>
    public TipoMensaje Tipo { get; set; } = TipoMensaje.Texto;
    /// <summary>Columna contenido (text); opcional.</summary>
    public string? Contenido { get; set; }
    /// <summary>Columna producto_id (uuid); opcional.</summary>
    public Guid? ProductoId { get; set; }
    /// <summary>Columna solicitud_cotizacion_id (uuid); opcional.</summary>
    public Guid? SolicitudCotizacionId { get; set; }
    /// <summary>Columna responde_a_mensaje_id (uuid); opcional.</summary>
    public Guid? RespondeAMensajeId { get; set; }
    /// <summary>Columna editado_en (timestamptz); opcional.</summary>
    public DateTime? EditadoEn { get; set; }
    /// <summary>Columna eliminado_en (timestamptz); opcional.</summary>
    public DateTime? EliminadoEn { get; set; }
    /// <summary>Relación hacia usuarios mediante usuario_remitente_id.</summary>
    [JsonIgnore]
    public Usuario? UsuarioRemitente { get; set; }
    /// <summary>Relación hacia productos mediante producto_id.</summary>
    [JsonIgnore]
    public Producto? Producto { get; set; }
    /// <summary>Relación hacia mensajes mediante responde_a_mensaje_id.</summary>
    [JsonIgnore]
    public Mensaje? RespondeAMensaje { get; set; }
    /// <summary>Relación hacia participantes mediante conversacion_id, perfil_remitente_id.</summary>
    [JsonIgnore]
    public Participante ParticipanteRemitente { get; set; } = null!;
    /// <summary>Relación hacia solicitudes_cotizacion mediante solicitud_cotizacion_id.</summary>
    [JsonIgnore]
    public SolicitudCotizacion? SolicitudCotizacion { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(ConversacionId != Guid.Empty, "ConversacionId es obligatorio.");
        Reglas.Exigir(PerfilRemitenteId != Guid.Empty, "PerfilRemitenteId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(Tipo), "Tipo contiene un valor no permitido.");
        Reglas.Texto(Contenido, nameof(Contenido), 10000, false, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(EditadoEn, nameof(EditadoEn));
        Reglas.FechaUtc(EliminadoEn, nameof(EliminadoEn));
    }

    /// <summary>Marca el registro como eliminado sin borrarlo físicamente.</summary>
    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }
}
