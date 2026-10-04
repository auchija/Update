using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla conversaciones.</summary>
public sealed class Conversacion : BaseEntity, IEntidadValidable
{
    /// <summary>Columna tipo (tipo_conversacion); obligatoria.</summary>
    public TipoConversacion Tipo { get; set; } = TipoConversacion.Directa;
    /// <summary>Columna titulo (text); opcional.</summary>
    public string? Titulo { get; set; }
    /// <summary>Columna clave_directa (text); opcional.</summary>
    public string? ClaveDirecta { get; set; }
    /// <summary>Columna creado_por_usuario_id (uuid); obligatoria.</summary>
    public Guid CreadoPorUsuarioId { get; set; }
    /// <summary>Columna ultimo_mensaje_id (uuid); opcional.</summary>
    public Guid? UltimoMensajeId { get; set; }
    /// <summary>Columna ultimo_mensaje_en (timestamptz); opcional.</summary>
    public DateTime? UltimoMensajeEn { get; set; }
    /// <summary>Relación hacia usuarios mediante creado_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    /// <summary>Relación hacia mensajes mediante ultimo_mensaje_id.</summary>
    [JsonIgnore]
    public Mensaje? UltimoMensaje { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(Enum.IsDefined(Tipo), "Tipo contiene un valor no permitido.");
        Reglas.Texto(Titulo, nameof(Titulo), 255, false, false);
        Reglas.Texto(ClaveDirecta, nameof(ClaveDirecta), 1024, false, false);
        Reglas.Exigir(CreadoPorUsuarioId != Guid.Empty, "CreadoPorUsuarioId es obligatorio.");
        Reglas.FechaUtc(UltimoMensajeEn, nameof(UltimoMensajeEn));
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
    }
}
