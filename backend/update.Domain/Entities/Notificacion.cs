using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Notificacion : BaseEntity, IEntidadValidable
{
    public Guid UsuarioDestinatarioId { get; set; }
    public TipoNotificacion Tipo { get; set; }
    public Guid? PerfilActorId { get; set; }
    public string? EntidadTipo { get; set; }
    public Guid? EntidadId { get; set; }
    public string? ClaveGrupo { get; set; }
    public JsonElement Datos { get; set; } = JsonSerializer.Deserialize<JsonElement>("{}");
    public DateTime? LeidoEn { get; set; }
    [JsonIgnore]
    public Usuario UsuarioDestinatario { get; set; } = null!;
    [JsonIgnore]
    public Perfil? PerfilActor { get; set; }

    public void Validar()
    {
        Reglas.Exigir(UsuarioDestinatarioId != Guid.Empty, "UsuarioDestinatarioId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(Tipo), "Tipo contiene un valor no permitido.");
        Reglas.Texto(EntidadTipo, nameof(EntidadTipo), 64, false, false);
        Reglas.Texto(ClaveGrupo, nameof(ClaveGrupo), 1024, false, false);
        Reglas.FechaUtc(LeidoEn, nameof(LeidoEn));
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }

public void MarcarLeida() => LeidoEn ??= DateTime.UtcNow;
}
