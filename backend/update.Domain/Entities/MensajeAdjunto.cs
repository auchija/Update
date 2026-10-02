using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla mensajes_adjuntos.</summary>
public sealed class MensajeAdjunto : IEntidadValidable
{
    /// <summary>Columna mensaje_id (uuid); obligatoria.</summary>
    public Guid MensajeId { get; set; }
    /// <summary>Columna archivo_id (uuid); obligatoria.</summary>
    public Guid ArchivoId { get; set; }
    /// <summary>Columna posicion (smallint); obligatoria.</summary>
    public short Posicion { get; set; }
    /// <summary>Relación hacia mensajes mediante mensaje_id.</summary>
    [JsonIgnore]
    public Mensaje Mensaje { get; set; } = null!;
    /// <summary>Relación hacia archivos mediante archivo_id.</summary>
    [JsonIgnore]
    public Archivo Archivo { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(MensajeId != Guid.Empty, "MensajeId es obligatorio.");
        Reglas.Exigir(ArchivoId != Guid.Empty, "ArchivoId es obligatorio.");
    }
}
