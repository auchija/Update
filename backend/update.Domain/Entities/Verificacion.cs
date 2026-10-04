using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla verificaciones.</summary>
public sealed class Verificacion : BaseEntity, IEntidadValidable
{
    /// <summary>Columna emprendimiento_id (uuid); obligatoria.</summary>
    public Guid EmprendimientoId { get; set; }
    /// <summary>Columna estado (estado_verificacion); obligatoria.</summary>
    public EstadoVerificacion Estado { get; set; } = EstadoVerificacion.Pendiente;
    /// <summary>Columna metodo (text); obligatoria.</summary>
    public string Metodo { get; set; } = "documentos";
    /// <summary>Columna enviado_por_usuario_id (uuid); obligatoria.</summary>
    public Guid EnviadoPorUsuarioId { get; set; }
    /// <summary>Columna revisado_por_usuario_id (uuid); opcional.</summary>
    public Guid? RevisadoPorUsuarioId { get; set; }
    /// <summary>Columna notas_revisor (text); opcional.</summary>
    public string? NotasRevisor { get; set; }
    /// <summary>Columna enviado_en (timestamptz); obligatoria.</summary>
    public DateTime EnviadoEn { get; set; } = DateTime.UtcNow;
    /// <summary>Columna revisado_en (timestamptz); opcional.</summary>
    public DateTime? RevisadoEn { get; set; }
    /// <summary>Relación hacia usuarios mediante revisado_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario? RevisadoPorUsuario { get; set; }
    /// <summary>Relación hacia emprendimientos mediante emprendimiento_id.</summary>
    [JsonIgnore]
    public Emprendimiento Emprendimiento { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante enviado_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario EnviadoPorUsuario { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(EmprendimientoId != Guid.Empty, "EmprendimientoId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.Texto(Metodo, nameof(Metodo), 64, true, false);
        Reglas.Exigir(EnviadoPorUsuarioId != Guid.Empty, "EnviadoPorUsuarioId es obligatorio.");
        Reglas.Texto(NotasRevisor, nameof(NotasRevisor), 4000, false, false);
        Reglas.FechaUtc(EnviadoEn, nameof(EnviadoEn));
        Reglas.FechaUtc(RevisadoEn, nameof(RevisadoEn));
    }
}
