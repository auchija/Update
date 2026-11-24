using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Sesion : BaseEntity, IEntidadValidable
{
    public Guid UsuarioId { get; set; }
    public string HashTokenRefresco { get; set; } = string.Empty;
    public Guid FamiliaId { get; set; }
    public Guid? ReemplazadoPorId { get; set; }
    public string? AgenteUsuario { get; set; }
    public IPAddress? DireccionIp { get; set; }
    public DateTime UltimoUsoEn { get; set; } = DateTime.UtcNow;
    public DateTime ExpiraEn { get; set; }
    public DateTime? RevocadoEn { get; set; }
    public string? MotivoRevocacion { get; set; }
    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;
    [JsonIgnore]
    public Sesion? ReemplazadoPor { get; set; }

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

public void Revocar(string motivo)
{
    Reglas.Texto(motivo, nameof(MotivoRevocacion), 255, true);
    RevocadoEn ??= DateTime.UtcNow;
    MotivoRevocacion = motivo;
}
}
