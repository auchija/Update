using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class UsuarioTipoPersona : IEntidadValidable
{
    public Guid UsuarioId { get; set; }
    public string TipoPersonaCodigo { get; set; } = string.Empty;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;
    [JsonIgnore]
    public TipoPersona TipoPersona { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(UsuarioId != Guid.Empty, "UsuarioId es obligatorio.");
        Reglas.Texto(TipoPersonaCodigo, nameof(TipoPersonaCodigo), 64, true, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }
}
