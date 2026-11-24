using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Mensaje : BaseEntity, IEntidadValidable, IEliminable
{
    public Guid ConversacionId { get; set; }
    public Guid PerfilRemitenteId { get; set; }
    public Guid? UsuarioRemitenteId { get; set; }
    public TipoMensaje Tipo { get; set; } = TipoMensaje.Texto;
    public string? Contenido { get; set; }
    public Guid? ProductoId { get; set; }
    public Guid? SolicitudCotizacionId { get; set; }
    public Guid? RespondeAMensajeId { get; set; }
    public DateTime? EditadoEn { get; set; }
    public DateTime? EliminadoEn { get; set; }
    [JsonIgnore]
    public Usuario? UsuarioRemitente { get; set; }
    [JsonIgnore]
    public Producto? Producto { get; set; }
    [JsonIgnore]
    public Mensaje? RespondeAMensaje { get; set; }
    [JsonIgnore]
    public Participante ParticipanteRemitente { get; set; } = null!;
    [JsonIgnore]
    public SolicitudCotizacion? SolicitudCotizacion { get; set; }

    public void Validar()
    {
        Reglas.Exigir(ConversacionId != Guid.Empty, "ConversacionId es obligatorio.");
        Reglas.Exigir(PerfilRemitenteId != Guid.Empty, "PerfilRemitenteId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(Tipo), "Tipo contiene un valor no permitido.");
        Reglas.Texto(Contenido, nameof(Contenido), 10000, false, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(EditadoEn, nameof(EditadoEn));
        Reglas.FechaUtc(EliminadoEn, nameof(EliminadoEn));
    }

    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }
}
