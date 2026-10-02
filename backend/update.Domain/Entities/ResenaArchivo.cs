using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla resenas_archivos.</summary>
public sealed class ResenaArchivo : IEntidadValidable
{
    /// <summary>Columna resena_id (uuid); obligatoria.</summary>
    public Guid ResenaId { get; set; }
    /// <summary>Columna archivo_id (uuid); obligatoria.</summary>
    public Guid ArchivoId { get; set; }
    /// <summary>Columna posicion (smallint); obligatoria.</summary>
    public short Posicion { get; set; }
    /// <summary>Relación hacia resenas mediante resena_id.</summary>
    [JsonIgnore]
    public Resena Resena { get; set; } = null!;
    /// <summary>Relación hacia archivos mediante archivo_id.</summary>
    [JsonIgnore]
    public Archivo Archivo { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(ResenaId != Guid.Empty, "ResenaId es obligatorio.");
        Reglas.Exigir(ArchivoId != Guid.Empty, "ArchivoId es obligatorio.");
    }
}
