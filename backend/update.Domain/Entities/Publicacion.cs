using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Publicacion : BaseEntity, IEntidadValidable, IEliminable
{
    public Guid PerfilAutorId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public string TipoPublicacionCodigo { get; set; } = "general";
    public string Contenido { get; set; } = "";
    public VisibilidadPublicacion Visibilidad { get; set; } = VisibilidadPublicacion.Publico;
    public Guid? ProductoId { get; set; }
    public Guid? OportunidadId { get; set; }
    public Guid? PublicacionCompartidaId { get; set; }
    public Guid? ComentarioAceptadoId { get; set; }
    public bool ComentariosHabilitados { get; set; } = true;
    public bool Fijado { get; set; } = false;
    public int TotalReacciones { get; set; } = 0;
    public int TotalComentarios { get; set; } = 0;
    public int TotalCompartidos { get; set; } = 0;
    public int TotalGuardados { get; set; } = 0;
    public double PuntajeDescubrimiento { get; set; } = 0d;
    public DateTime? PuntajeActualizadoEn { get; set; }
    public DateTime? EditadoEn { get; set; }
    public DateTime? EliminadoEn { get; set; }
    [JsonIgnore]
    public Perfil PerfilAutor { get; set; } = null!;
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    [JsonIgnore]
    public TipoPublicacion TipoPublicacion { get; set; } = null!;
    [JsonIgnore]
    public Producto? Producto { get; set; }
    [JsonIgnore]
    public Oportunidad? Oportunidad { get; set; }
    [JsonIgnore]
    public Publicacion? PublicacionCompartida { get; set; }
    [JsonIgnore]
    public Comentario? ComentarioAceptado { get; set; }

    public void Validar()
    {
        Reglas.Exigir(PerfilAutorId != Guid.Empty, "PerfilAutorId es obligatorio.");
        Reglas.Exigir(CreadoPorUsuarioId != Guid.Empty, "CreadoPorUsuarioId es obligatorio.");
        Reglas.Texto(TipoPublicacionCodigo, nameof(TipoPublicacionCodigo), 64, true, false);
        Reglas.Texto(Contenido, nameof(Contenido), 10000, true, true);
        Reglas.Exigir(Enum.IsDefined(Visibilidad), "Visibilidad contiene un valor no permitido.");
        Reglas.Exigir(TotalReacciones >= 0, "TotalReacciones no puede ser negativo.");
        Reglas.Exigir(TotalComentarios >= 0, "TotalComentarios no puede ser negativo.");
        Reglas.Exigir(TotalCompartidos >= 0, "TotalCompartidos no puede ser negativo.");
        Reglas.Exigir(TotalGuardados >= 0, "TotalGuardados no puede ser negativo.");
        Reglas.FechaUtc(PuntajeActualizadoEn, nameof(PuntajeActualizadoEn));
        Reglas.FechaUtc(EditadoEn, nameof(EditadoEn));
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
