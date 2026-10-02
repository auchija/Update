using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla comentarios.</summary>
public sealed class Comentario : BaseEntity, IEntidadValidable, IEliminable
{
    /// <summary>Columna publicacion_id (uuid); obligatoria.</summary>
    public Guid PublicacionId { get; set; }
    /// <summary>Columna comentario_padre_id (uuid); opcional.</summary>
    public Guid? ComentarioPadreId { get; set; }
    /// <summary>Columna perfil_autor_id (uuid); obligatoria.</summary>
    public Guid PerfilAutorId { get; set; }
    /// <summary>Columna creado_por_usuario_id (uuid); obligatoria.</summary>
    public Guid CreadoPorUsuarioId { get; set; }
    /// <summary>Columna responde_a_perfil_id (uuid); opcional.</summary>
    public Guid? RespondeAPerfilId { get; set; }
    /// <summary>Columna contenido (text); obligatoria.</summary>
    public string Contenido { get; set; } = string.Empty;
    /// <summary>Columna total_reacciones (integer); obligatoria.</summary>
    public int TotalReacciones { get; set; } = 0;
    /// <summary>Columna total_respuestas (integer); obligatoria.</summary>
    public int TotalRespuestas { get; set; } = 0;
    /// <summary>Columna editado_en (timestamptz); opcional.</summary>
    public DateTime? EditadoEn { get; set; }
    /// <summary>Columna eliminado_en (timestamptz); opcional.</summary>
    public DateTime? EliminadoEn { get; set; }
    /// <summary>Relación hacia publicaciones mediante publicacion_id.</summary>
    [JsonIgnore]
    public Publicacion Publicacion { get; set; } = null!;
    /// <summary>Relación hacia perfiles mediante perfil_autor_id.</summary>
    [JsonIgnore]
    public Perfil PerfilAutor { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante creado_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    /// <summary>Relación hacia perfiles mediante responde_a_perfil_id.</summary>
    [JsonIgnore]
    public Perfil? RespondeAPerfil { get; set; }
    /// <summary>Relación hacia comentarios mediante comentario_padre_id, publicacion_id.</summary>
    [JsonIgnore]
    public Comentario? ComentarioPadre { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
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

    /// <summary>Marca el registro como eliminado sin borrarlo físicamente.</summary>
    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }
}
