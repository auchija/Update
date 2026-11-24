using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class ReaccionPublicacion : IEntidadValidable
{
    public Guid PublicacionId { get; set; }
    public Guid PerfilId { get; set; }
    public string TipoReaccionCodigo { get; set; } = string.Empty;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public Publicacion Publicacion { get; set; } = null!;
    [JsonIgnore]
    public Perfil Perfil { get; set; } = null!;
    [JsonIgnore]
    public TipoReaccion TipoReaccion { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(PublicacionId != Guid.Empty, "PublicacionId es obligatorio.");
        Reglas.Exigir(PerfilId != Guid.Empty, "PerfilId es obligatorio.");
        Reglas.Texto(TipoReaccionCodigo, nameof(TipoReaccionCodigo), 64, true, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
    }
}
