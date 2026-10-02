using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla solicitudes_mentoria.</summary>
public sealed class SolicitudMentoria : BaseEntity, IEntidadValidable
{
    /// <summary>Columna oferta_id (uuid); obligatoria.</summary>
    public Guid OfertaId { get; set; }
    /// <summary>Columna perfil_mentoreado_id (uuid); obligatoria.</summary>
    public Guid PerfilMentoreadoId { get; set; }
    /// <summary>Columna creado_por_usuario_id (uuid); obligatoria.</summary>
    public Guid CreadoPorUsuarioId { get; set; }
    /// <summary>Columna mensaje (text); obligatoria.</summary>
    public string Mensaje { get; set; } = string.Empty;
    /// <summary>Columna estado (estado_mentoria); obligatoria.</summary>
    public EstadoMentoria Estado { get; set; } = EstadoMentoria.Solicitado;
    /// <summary>Columna agendado_en (timestamptz); opcional.</summary>
    public DateTime? AgendadoEn { get; set; }
    /// <summary>Columna url_reunion (text); opcional.</summary>
    public string? UrlReunion { get; set; }
    /// <summary>Columna calificacion_mentoreado (smallint); opcional.</summary>
    public short? CalificacionMentoreado { get; set; }
    /// <summary>Columna comentario_mentoreado (text); opcional.</summary>
    public string? ComentarioMentoreado { get; set; }
    /// <summary>Relación hacia ofertas_mentoria mediante oferta_id.</summary>
    [JsonIgnore]
    public OfertaMentoria Oferta { get; set; } = null!;
    /// <summary>Relación hacia perfiles mediante perfil_mentoreado_id.</summary>
    [JsonIgnore]
    public Perfil PerfilMentoreado { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante creado_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(OfertaId != Guid.Empty, "OfertaId es obligatorio.");
        Reglas.Exigir(PerfilMentoreadoId != Guid.Empty, "PerfilMentoreadoId es obligatorio.");
        Reglas.Exigir(CreadoPorUsuarioId != Guid.Empty, "CreadoPorUsuarioId es obligatorio.");
        Reglas.Texto(Mensaje, nameof(Mensaje), 10000, true, false);
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.FechaUtc(AgendadoEn, nameof(AgendadoEn));
        Reglas.Texto(UrlReunion, nameof(UrlReunion), 2048, false, false);
        Reglas.Exigir(CalificacionMentoreado is null || CalificacionMentoreado is >= 1 and <= 5, "CalificacionMentoreado debe estar entre 1 y 5.");
        Reglas.Texto(ComentarioMentoreado, nameof(ComentarioMentoreado), 4000, false, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
    }
}
