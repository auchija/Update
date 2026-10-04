using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla sesiones.</summary>
public sealed class Sesion : BaseEntity, IEntidadValidable
{
    /// <summary>Columna usuario_id (uuid); obligatoria.</summary>
    public Guid UsuarioId { get; set; }
    /// <summary>Columna hash_token_refresco (text); obligatoria.</summary>
    public string HashTokenRefresco { get; set; } = string.Empty;
    /// <summary>Columna familia_id (uuid); obligatoria.</summary>
    public Guid FamiliaId { get; set; }
    /// <summary>Columna reemplazado_por_id (uuid); opcional.</summary>
    public Guid? ReemplazadoPorId { get; set; }
    /// <summary>Columna agente_usuario (text); opcional.</summary>
    public string? AgenteUsuario { get; set; }
    /// <summary>Columna direccion_ip (inet); opcional.</summary>
    public IPAddress? DireccionIp { get; set; }
    /// <summary>Columna ultimo_uso_en (timestamptz); obligatoria.</summary>
    public DateTime UltimoUsoEn { get; set; } = DateTime.UtcNow;
    /// <summary>Columna expira_en (timestamptz); obligatoria.</summary>
    public DateTime ExpiraEn { get; set; }
    /// <summary>Columna revocado_en (timestamptz); opcional.</summary>
    public DateTime? RevocadoEn { get; set; }
    /// <summary>Columna motivo_revocacion (text); opcional.</summary>
    public string? MotivoRevocacion { get; set; }
    /// <summary>Relación hacia usuarios mediante usuario_id.</summary>
    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;
    /// <summary>Relación hacia sesiones mediante reemplazado_por_id.</summary>
    [JsonIgnore]
    public Sesion? ReemplazadoPor { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(UsuarioId != Guid.Empty, "UsuarioId es obligatorio.");
        Reglas.Texto(HashTokenRefresco, nameof(HashTokenRefresco), 256, true, false);
        Reglas.Exigir(FamiliaId != Guid.Empty, "FamiliaId es obligatorio.");
        Reglas.Texto(AgenteUsuario, nameof(AgenteUsuario), 255, false, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(UltimoUsoEn, nameof(UltimoUsoEn));
        Reglas.FechaUtc(ExpiraEn, nameof(ExpiraEn));
        Reglas.FechaUtc(RevocadoEn, nameof(RevocadoEn));
        Reglas.Texto(MotivoRevocacion, nameof(MotivoRevocacion), 255, false, false);
    }

/// <summary>Revoca la sesión conservando la evidencia de su creación.</summary>
public void Revocar(string motivo)
{
    Reglas.Texto(motivo, nameof(MotivoRevocacion), 255, true);
    RevocadoEn ??= DateTime.UtcNow;
    MotivoRevocacion = motivo;
}
}
