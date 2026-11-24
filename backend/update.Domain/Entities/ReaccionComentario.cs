using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class ReaccionComentario : IEntidadValidable
{
    public Guid ComentarioId { get; set; }
    public Guid PerfilId { get; set; }
    public string TipoReaccionCodigo { get; set; } = string.Empty;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public Comentario Comentario { get; set; } = null!;
    [JsonIgnore]
    public Perfil Perfil { get; set; } = null!;
    [JsonIgnore]
    public TipoReaccion TipoReaccion { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(ComentarioId != Guid.Empty, "ComentarioId es obligatorio.");
        Reglas.Exigir(PerfilId != Guid.Empty, "PerfilId es obligatorio.");
        Reglas.Texto(TipoReaccionCodigo, nameof(TipoReaccionCodigo), 64, true, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }
}
