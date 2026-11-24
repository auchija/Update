using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class HistorialSolicitud : IEntidadValidable
{
    public long Id { get; set; }
    public Guid SolicitudCotizacionId { get; set; }
    public EstadoSolicitud? EstadoAnterior { get; set; }
    public EstadoSolicitud EstadoNuevo { get; set; }
    public Guid? UsuarioActorId { get; set; }
    public string? Nota { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public SolicitudCotizacion SolicitudCotizacion { get; set; } = null!;
    [JsonIgnore]
    public Usuario? UsuarioActor { get; set; }

    public void Validar()
    {
        Reglas.Exigir(SolicitudCotizacionId != Guid.Empty, "SolicitudCotizacionId es obligatorio.");
        Reglas.Exigir(EstadoAnterior is null || Enum.IsDefined(EstadoAnterior.Value), "EstadoAnterior contiene un valor no permitido.");
        Reglas.Exigir(Enum.IsDefined(EstadoNuevo), "EstadoNuevo contiene un valor no permitido.");
        Reglas.Texto(Nota, nameof(Nota), 4000, false, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }
}
