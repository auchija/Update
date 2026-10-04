using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla reacciones_comentario.</summary>
public sealed class ReaccionComentario : IEntidadValidable
{
    /// <summary>Columna comentario_id (uuid); obligatoria.</summary>
    public Guid ComentarioId { get; set; }
    /// <summary>Columna perfil_id (uuid); obligatoria.</summary>
    public Guid PerfilId { get; set; }
    /// <summary>Columna tipo_reaccion_codigo (text); obligatoria.</summary>
    public string TipoReaccionCodigo { get; set; } = string.Empty;
    /// <summary>Columna creado_en (timestamptz); obligatoria.</summary>
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    /// <summary>Relación hacia comentarios mediante comentario_id.</summary>
    [JsonIgnore]
    public Comentario Comentario { get; set; } = null!;
    /// <summary>Relación hacia perfiles mediante perfil_id.</summary>
    [JsonIgnore]
    public Perfil Perfil { get; set; } = null!;
    /// <summary>Relación hacia tipos_reaccion mediante tipo_reaccion_codigo.</summary>
    [JsonIgnore]
    public TipoReaccion TipoReaccion { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(ComentarioId != Guid.Empty, "ComentarioId es obligatorio.");
        Reglas.Exigir(PerfilId != Guid.Empty, "PerfilId es obligatorio.");
        Reglas.Texto(TipoReaccionCodigo, nameof(TipoReaccionCodigo), 64, true, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }
}
