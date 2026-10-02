using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla productos_archivos.</summary>
public sealed class ProductoArchivo : IEntidadValidable
{
    /// <summary>Columna producto_id (uuid); obligatoria.</summary>
    public Guid ProductoId { get; set; }
    /// <summary>Columna archivo_id (uuid); obligatoria.</summary>
    public Guid ArchivoId { get; set; }
    /// <summary>Columna posicion (smallint); obligatoria.</summary>
    public short Posicion { get; set; }
    /// <summary>Columna texto_alternativo (text); opcional.</summary>
    public string? TextoAlternativo { get; set; }
    /// <summary>Relación hacia productos mediante producto_id.</summary>
    [JsonIgnore]
    public Producto Producto { get; set; } = null!;
    /// <summary>Relación hacia archivos mediante archivo_id.</summary>
    [JsonIgnore]
    public Archivo Archivo { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(ProductoId != Guid.Empty, "ProductoId es obligatorio.");
        Reglas.Exigir(ArchivoId != Guid.Empty, "ArchivoId es obligatorio.");
        Reglas.Texto(TextoAlternativo, nameof(TextoAlternativo), 255, false, false);
    }
}
