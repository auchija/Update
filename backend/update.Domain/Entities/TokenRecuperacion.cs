using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class TokenRecuperacion : BaseEntity, IEntidadValidable
{
    public Guid UsuarioId { get; set; }
    public string HashToken { get; set; } = string.Empty;
    public IPAddress? IpSolicitud { get; set; }
    public DateTime ExpiraEn { get; set; }
    public DateTime? UsadoEn { get; set; }
    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(UsuarioId != Guid.Empty, "UsuarioId es obligatorio.");
        Reglas.Texto(HashToken, nameof(HashToken), 256, true, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ExpiraEn, nameof(ExpiraEn));
        Reglas.FechaUtc(UsadoEn, nameof(UsadoEn));
    }

public void MarcarUsado()
{
    Reglas.Exigir(UsadoEn is null && ExpiraEn > DateTime.UtcNow, "El token ya fue usado o expiró.");
    UsadoEn = DateTime.UtcNow;
}
}
