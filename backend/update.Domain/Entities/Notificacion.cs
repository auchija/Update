using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla notificaciones.</summary>
public sealed class Notificacion : BaseEntity, IEntidadValidable
{
    /// <summary>Columna usuario_destinatario_id (uuid); obligatoria.</summary>
    public Guid UsuarioDestinatarioId { get; set; }
    /// <summary>Columna tipo (tipo_notificacion); obligatoria.</summary>
    public TipoNotificacion Tipo { get; set; }
    /// <summary>Columna perfil_actor_id (uuid); opcional.</summary>
    public Guid? PerfilActorId { get; set; }
    /// <summary>Columna entidad_tipo (text); opcional.</summary>
    public string? EntidadTipo { get; set; }
    /// <summary>Columna entidad_id (uuid); opcional.</summary>
    public Guid? EntidadId { get; set; }
    /// <summary>Columna clave_grupo (text); opcional.</summary>
    public string? ClaveGrupo { get; set; }
    /// <summary>Columna datos (jsonb); obligatoria.</summary>
    public JsonElement Datos { get; set; } = JsonSerializer.Deserialize<JsonElement>("{}");
    /// <summary>Columna leido_en (timestamptz); opcional.</summary>
    public DateTime? LeidoEn { get; set; }
    /// <summary>Relación hacia usuarios mediante usuario_destinatario_id.</summary>
    [JsonIgnore]
    public Usuario UsuarioDestinatario { get; set; } = null!;
    /// <summary>Relación hacia perfiles mediante perfil_actor_id.</summary>
    [JsonIgnore]
    public Perfil? PerfilActor { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(UsuarioDestinatarioId != Guid.Empty, "UsuarioDestinatarioId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(Tipo), "Tipo contiene un valor no permitido.");
        Reglas.Texto(EntidadTipo, nameof(EntidadTipo), 64, false, false);
        Reglas.Texto(ClaveGrupo, nameof(ClaveGrupo), 1024, false, false);
        Reglas.FechaUtc(LeidoEn, nameof(LeidoEn));
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }

/// <summary>Marca la notificación como leída una sola vez.</summary>
public void MarcarLeida() => LeidoEn ??= DateTime.UtcNow;
}
