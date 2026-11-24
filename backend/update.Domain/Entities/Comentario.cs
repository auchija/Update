using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Comentario : BaseEntity, IEntidadValidable, IEliminable
{
    public Guid PublicacionId { get; set; }
    public Guid? ComentarioPadreId { get; set; }
    public Guid PerfilAutorId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public Guid? RespondeAPerfilId { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public int TotalReacciones { get; set; } = 0;
    public int TotalRespuestas { get; set; } = 0;
    public DateTime? EditadoEn { get; set; }
    public DateTime? EliminadoEn { get; set; }
    [JsonIgnore]
    public Publicacion Publicacion { get; set; } = null!;
    [JsonIgnore]
    public Perfil PerfilAutor { get; set; } = null!;
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    [JsonIgnore]
    public Perfil? RespondeAPerfil { get; set; }
    [JsonIgnore]
    public Comentario? ComentarioPadre { get; set; }

    public void Validar()
    {
        Reglas.Exigir(PublicacionId != Guid.Empty, "PublicacionId es obligatorio.");
        Reglas.Exigir(PerfilAutorId != Guid.Empty, "PerfilAutorId es obligatorio.");
        Reglas.Exigir(CreadoPorUsuarioId != Guid.Empty, "CreadoPorUsuarioId es obligatorio.");
        Reglas.Texto(Contenido, nameof(Contenido), 10000, true, false);
        Reglas.Exigir(TotalReacciones >= 0, "TotalReacciones no puede ser negativo.");
        Reglas.Exigir(TotalRespuestas >= 0, "TotalRespuestas no puede ser negativo.");
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
