using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla participantes.</summary>
public sealed class Participante : IEntidadValidable
{
    /// <summary>Columna conversacion_id (uuid); obligatoria.</summary>
    public Guid ConversacionId { get; set; }
    /// <summary>Columna perfil_id (uuid); obligatoria.</summary>
    public Guid PerfilId { get; set; }
    /// <summary>Columna rol (rol_participante); obligatoria.</summary>
    public RolParticipante Rol { get; set; } = RolParticipante.Miembro;
    /// <summary>Columna usuario_asignado_id (uuid); opcional.</summary>
    public Guid? UsuarioAsignadoId { get; set; }
    /// <summary>Columna unido_en (timestamptz); obligatoria.</summary>
    public DateTime UnidoEn { get; set; } = DateTime.UtcNow;
    /// <summary>Columna salio_en (timestamptz); opcional.</summary>
    public DateTime? SalioEn { get; set; }
    /// <summary>Columna ultimo_mensaje_leido_id (uuid); opcional.</summary>
    public Guid? UltimoMensajeLeidoId { get; set; }
    /// <summary>Columna ultima_lectura_en (timestamptz); opcional.</summary>
    public DateTime? UltimaLecturaEn { get; set; }
    /// <summary>Columna silenciado_hasta (timestamptz); opcional.</summary>
    public DateTime? SilenciadoHasta { get; set; }
    /// <summary>Columna archivado_en (timestamptz); opcional.</summary>
    public DateTime? ArchivadoEn { get; set; }
    /// <summary>Relación hacia conversaciones mediante conversacion_id.</summary>
    [JsonIgnore]
    public Conversacion Conversacion { get; set; } = null!;
    /// <summary>Relación hacia perfiles mediante perfil_id.</summary>
    [JsonIgnore]
    public Perfil Perfil { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante usuario_asignado_id.</summary>
    [JsonIgnore]
    public Usuario? UsuarioAsignado { get; set; }
    /// <summary>Relación hacia mensajes mediante ultimo_mensaje_leido_id.</summary>
    [JsonIgnore]
    public Mensaje? UltimoMensajeLeido { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(ConversacionId != Guid.Empty, "ConversacionId es obligatorio.");
        Reglas.Exigir(PerfilId != Guid.Empty, "PerfilId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(Rol), "Rol contiene un valor no permitido.");
        Reglas.FechaUtc(UnidoEn, nameof(UnidoEn));
        Reglas.FechaUtc(SalioEn, nameof(SalioEn));
        Reglas.FechaUtc(UltimaLecturaEn, nameof(UltimaLecturaEn));
        Reglas.FechaUtc(SilenciadoHasta, nameof(SilenciadoHasta));
        Reglas.FechaUtc(ArchivadoEn, nameof(ArchivadoEn));
    }
}
