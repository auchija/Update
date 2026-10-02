using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla variantes_archivo.</summary>
public sealed class VarianteArchivo : IEntidadValidable
{
    /// <summary>Columna archivo_id (uuid); obligatoria.</summary>
    public Guid ArchivoId { get; set; }
    /// <summary>Columna variante (text); obligatoria.</summary>
    public string Variante { get; set; } = string.Empty;
    /// <summary>Columna clave_objeto (text); obligatoria.</summary>
    public string ClaveObjeto { get; set; } = string.Empty;
    /// <summary>Columna tipo_mime (text); obligatoria.</summary>
    public string TipoMime { get; set; } = string.Empty;
    /// <summary>Columna ancho (integer); opcional.</summary>
    public int? Ancho { get; set; }
    /// <summary>Columna alto (integer); opcional.</summary>
    public int? Alto { get; set; }
    /// <summary>Columna tamano_bytes (bigint); obligatoria.</summary>
    public long TamanoBytes { get; set; }
    /// <summary>Relación hacia archivos mediante archivo_id.</summary>
    [JsonIgnore]
    public Archivo Archivo { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(ArchivoId != Guid.Empty, "ArchivoId es obligatorio.");
        Reglas.Texto(Variante, nameof(Variante), 64, true, false);
        Reglas.Texto(ClaveObjeto, nameof(ClaveObjeto), 1024, true, false);
        Reglas.Texto(TipoMime, nameof(TipoMime), 127, true, false);
        Reglas.Exigir(TamanoBytes >= 0, "TamanoBytes no puede ser negativo.");
    }
}
