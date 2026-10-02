using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla usuario_tipos_persona.</summary>
public sealed class UsuarioTipoPersona : IEntidadValidable
{
    /// <summary>Columna usuario_id (uuid); obligatoria.</summary>
    public Guid UsuarioId { get; set; }
    /// <summary>Columna tipo_persona_codigo (text); obligatoria.</summary>
    public string TipoPersonaCodigo { get; set; } = string.Empty;
    /// <summary>Columna creado_en (timestamptz); obligatoria.</summary>
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    /// <summary>Relación hacia usuarios mediante usuario_id.</summary>
    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;
    /// <summary>Relación hacia tipos_persona mediante tipo_persona_codigo.</summary>
    [JsonIgnore]
    public TipoPersona TipoPersona { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(UsuarioId != Guid.Empty, "UsuarioId es obligatorio.");
        Reglas.Texto(TipoPersonaCodigo, nameof(TipoPersonaCodigo), 64, true, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }
}
