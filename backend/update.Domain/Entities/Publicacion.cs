using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla publicaciones.</summary>
public sealed class Publicacion : BaseEntity, IEntidadValidable, IEliminable
{
    /// <summary>Columna perfil_autor_id (uuid); obligatoria.</summary>
    public Guid PerfilAutorId { get; set; }
    /// <summary>Columna creado_por_usuario_id (uuid); obligatoria.</summary>
    public Guid CreadoPorUsuarioId { get; set; }
    /// <summary>Columna tipo_publicacion_codigo (text); obligatoria.</summary>
    public string TipoPublicacionCodigo { get; set; } = "general";
    /// <summary>Columna contenido (text); obligatoria.</summary>
    public string Contenido { get; set; } = "";
    /// <summary>Columna visibilidad (visibilidad_publicacion); obligatoria.</summary>
    public VisibilidadPublicacion Visibilidad { get; set; } = VisibilidadPublicacion.Publico;
    /// <summary>Columna producto_id (uuid); opcional.</summary>
    public Guid? ProductoId { get; set; }
    /// <summary>Columna oportunidad_id (uuid); opcional.</summary>
    public Guid? OportunidadId { get; set; }
    /// <summary>Columna publicacion_compartida_id (uuid); opcional.</summary>
    public Guid? PublicacionCompartidaId { get; set; }
    /// <summary>Columna comentario_aceptado_id (uuid); opcional.</summary>
    public Guid? ComentarioAceptadoId { get; set; }
    /// <summary>Columna comentarios_habilitados (boolean); obligatoria.</summary>
    public bool ComentariosHabilitados { get; set; } = true;
    /// <summary>Columna fijado (boolean); obligatoria.</summary>
    public bool Fijado { get; set; } = false;
    /// <summary>Columna total_reacciones (integer); obligatoria.</summary>
    public int TotalReacciones { get; set; } = 0;
    /// <summary>Columna total_comentarios (integer); obligatoria.</summary>
    public int TotalComentarios { get; set; } = 0;
    /// <summary>Columna total_compartidos (integer); obligatoria.</summary>
    public int TotalCompartidos { get; set; } = 0;
    /// <summary>Columna total_guardados (integer); obligatoria.</summary>
    public int TotalGuardados { get; set; } = 0;
    /// <summary>Columna puntaje_descubrimiento (float8); obligatoria.</summary>
    public double PuntajeDescubrimiento { get; set; } = 0d;
    /// <summary>Columna puntaje_actualizado_en (timestamptz); opcional.</summary>
    public DateTime? PuntajeActualizadoEn { get; set; }
    /// <summary>Columna editado_en (timestamptz); opcional.</summary>
    public DateTime? EditadoEn { get; set; }
    /// <summary>Columna eliminado_en (timestamptz); opcional.</summary>
    public DateTime? EliminadoEn { get; set; }
    /// <summary>Relación hacia perfiles mediante perfil_autor_id.</summary>
    [JsonIgnore]
    public Perfil PerfilAutor { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante creado_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    /// <summary>Relación hacia tipos_publicacion mediante tipo_publicacion_codigo.</summary>
    [JsonIgnore]
    public TipoPublicacion TipoPublicacion { get; set; } = null!;
    /// <summary>Relación hacia productos mediante producto_id.</summary>
    [JsonIgnore]
    public Producto? Producto { get; set; }
    /// <summary>Relación hacia oportunidades mediante oportunidad_id.</summary>
    [JsonIgnore]
    public Oportunidad? Oportunidad { get; set; }
    /// <summary>Relación hacia publicaciones mediante publicacion_compartida_id.</summary>
    [JsonIgnore]
    public Publicacion? PublicacionCompartida { get; set; }
    /// <summary>Relación hacia comentarios mediante comentario_aceptado_id, id.</summary>
    [JsonIgnore]
    public Comentario? ComentarioAceptado { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
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

    /// <summary>Marca el registro como eliminado sin borrarlo físicamente.</summary>
    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
    }
}
