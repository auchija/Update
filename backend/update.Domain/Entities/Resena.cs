using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Resena : BaseEntity, IEntidadValidable, IEliminable
{
    public Guid UsuarioResenadorId { get; set; }
    public Guid EmprendimientoId { get; set; }
    public Guid? ProductoId { get; set; }
    public Guid? SolicitudCotizacionId { get; set; }
    public short Calificacion { get; set; }
    public string? Titulo { get; set; }
    public string? Contenido { get; set; }
    public EstadoResena Estado { get; set; } = EstadoResena.Publicado;
    public string? RespuestaEmprendimiento { get; set; }
    public Guid? RespondidoPorUsuarioId { get; set; }
    public DateTime? RespondidoEn { get; set; }
    public DateTime? EliminadoEn { get; set; }
    [JsonIgnore]
    public Usuario UsuarioResenador { get; set; } = null!;
    [JsonIgnore]
    public Emprendimiento Emprendimiento { get; set; } = null!;
    [JsonIgnore]
    public Usuario? RespondidoPorUsuario { get; set; }
    [JsonIgnore]
    public Producto? Producto { get; set; }
    [JsonIgnore]
    public SolicitudCotizacion? SolicitudCotizacion { get; set; }

    public void Validar()
    {
        Reglas.Exigir(UsuarioResenadorId != Guid.Empty, "UsuarioResenadorId es obligatorio.");
        Reglas.Exigir(EmprendimientoId != Guid.Empty, "EmprendimientoId es obligatorio.");
        Reglas.Exigir(Calificacion is >= 1 and <= 5, "Calificacion debe estar entre 1 y 5.");
        Reglas.Texto(Titulo, nameof(Titulo), 255, false, false);
        Reglas.Texto(Contenido, nameof(Contenido), 10000, false, false);
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.Texto(RespuestaEmprendimiento, nameof(RespuestaEmprendimiento), 10000, false, false);
        Reglas.FechaUtc(RespondidoEn, nameof(RespondidoEn));
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
        Reglas.FechaUtc(EliminadoEn, nameof(EliminadoEn));
    }

    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }
}
