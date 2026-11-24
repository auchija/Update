using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class SolicitudCotizacion : BaseEntity, IEntidadValidable
{
    public long Numero { get; set; }
    public Guid EmprendimientoId { get; set; }
    public Guid PerfilSolicitanteId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public Guid? ConversacionId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Mensaje { get; set; }
    public DateOnly? FechaDeseada { get; set; }
    public int? CiudadEntregaId { get; set; }
    public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;
    public Guid? CotizacionAceptadaId { get; set; }
    public DateTime? CerradoEn { get; set; }
    [JsonIgnore]
    public Emprendimiento Emprendimiento { get; set; } = null!;
    [JsonIgnore]
    public Perfil PerfilSolicitante { get; set; } = null!;
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    [JsonIgnore]
    public Conversacion? Conversacion { get; set; }
    [JsonIgnore]
    public Ciudad? CiudadEntrega { get; set; }
    [JsonIgnore]
    public Cotizacion? CotizacionAceptada { get; set; }

    public void Validar()
    {
        Reglas.Exigir(EmprendimientoId != Guid.Empty, "EmprendimientoId es obligatorio.");
        Reglas.Exigir(PerfilSolicitanteId != Guid.Empty, "PerfilSolicitanteId es obligatorio.");
        Reglas.Exigir(CreadoPorUsuarioId != Guid.Empty, "CreadoPorUsuarioId es obligatorio.");
        Reglas.Texto(Titulo, nameof(Titulo), 255, true, false);
        Reglas.Texto(Mensaje, nameof(Mensaje), 10000, false, false);
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
        Reglas.FechaUtc(CerradoEn, nameof(CerradoEn));
    }
}
