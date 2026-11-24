using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Conversacion : BaseEntity, IEntidadValidable
{
    public TipoConversacion Tipo { get; set; } = TipoConversacion.Directa;
    public string? Titulo { get; set; }
    public string? ClaveDirecta { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public Guid? UltimoMensajeId { get; set; }
    public DateTime? UltimoMensajeEn { get; set; }
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    [JsonIgnore]
    public Mensaje? UltimoMensaje { get; set; }

    public void Validar()
    {
        Reglas.Exigir(Enum.IsDefined(Tipo), "Tipo contiene un valor no permitido.");
        Reglas.Texto(Titulo, nameof(Titulo), 255, false, false);
        Reglas.Texto(ClaveDirecta, nameof(ClaveDirecta), 1024, false, false);
        Reglas.Exigir(CreadoPorUsuarioId != Guid.Empty, "CreadoPorUsuarioId es obligatorio.");
        Reglas.FechaUtc(UltimoMensajeEn, nameof(UltimoMensajeEn));
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
    }
}
