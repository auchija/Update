using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class SolicitudMentoria : BaseEntity, IEntidadValidable
{
    public Guid OfertaId { get; set; }
    public Guid PerfilMentoreadoId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public EstadoMentoria Estado { get; set; } = EstadoMentoria.Solicitado;
    public DateTime? AgendadoEn { get; set; }
    public string? UrlReunion { get; set; }
    public short? CalificacionMentoreado { get; set; }
    public string? ComentarioMentoreado { get; set; }
    [JsonIgnore]
    public OfertaMentoria Oferta { get; set; } = null!;
    [JsonIgnore]
    public Perfil PerfilMentoreado { get; set; } = null!;
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;

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
