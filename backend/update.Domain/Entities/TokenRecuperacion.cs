using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla tokens_recuperacion.</summary>
public sealed class TokenRecuperacion : BaseEntity, IEntidadValidable
{
    /// <summary>Columna usuario_id (uuid); obligatoria.</summary>
    public Guid UsuarioId { get; set; }
    /// <summary>Columna hash_token (text); obligatoria.</summary>
    public string HashToken { get; set; } = string.Empty;
    /// <summary>Columna ip_solicitud (inet); opcional.</summary>
    public IPAddress? IpSolicitud { get; set; }
    /// <summary>Columna expira_en (timestamptz); obligatoria.</summary>
    public DateTime ExpiraEn { get; set; }
    /// <summary>Columna usado_en (timestamptz); opcional.</summary>
    public DateTime? UsadoEn { get; set; }
    /// <summary>Relación hacia usuarios mediante usuario_id.</summary>
    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(UsuarioId != Guid.Empty, "UsuarioId es obligatorio.");
        Reglas.Texto(HashToken, nameof(HashToken), 256, true, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ExpiraEn, nameof(ExpiraEn));
        Reglas.FechaUtc(UsadoEn, nameof(UsadoEn));
    }

/// <summary>Consume un token vigente; su hash nunca se devuelve en el DTO.</summary>
public void MarcarUsado()
{
    Reglas.Exigir(UsadoEn is null && ExpiraEn > DateTime.UtcNow, "El token ya fue usado o expiró.");
    UsadoEn = DateTime.UtcNow;
}
}
