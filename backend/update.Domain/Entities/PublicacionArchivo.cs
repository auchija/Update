using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla publicaciones_archivos.</summary>
public sealed class PublicacionArchivo : IEntidadValidable
{
    /// <summary>Columna publicacion_id (uuid); obligatoria.</summary>
    public Guid PublicacionId { get; set; }
    /// <summary>Columna archivo_id (uuid); obligatoria.</summary>
    public Guid ArchivoId { get; set; }
    /// <summary>Columna posicion (smallint); obligatoria.</summary>
    public short Posicion { get; set; }
    /// <summary>Columna texto_alternativo (text); opcional.</summary>
    public string? TextoAlternativo { get; set; }
    /// <summary>Relación hacia archivos mediante archivo_id.</summary>
    [JsonIgnore]
    public Archivo Archivo { get; set; } = null!;
    /// <summary>Relación hacia publicaciones mediante publicacion_id.</summary>
    [JsonIgnore]
    public Publicacion Publicacion { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(PublicacionId != Guid.Empty, "PublicacionId es obligatorio.");
        Reglas.Exigir(ArchivoId != Guid.Empty, "ArchivoId es obligatorio.");
        Reglas.Texto(TextoAlternativo, nameof(TextoAlternativo), 255, false, false);
    }
}
