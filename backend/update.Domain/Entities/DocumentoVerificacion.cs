using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla documentos_verificacion.</summary>
public sealed class DocumentoVerificacion : IEntidadValidable
{
    /// <summary>Columna verificacion_id (uuid); obligatoria.</summary>
    public Guid VerificacionId { get; set; }
    /// <summary>Columna archivo_id (uuid); obligatoria.</summary>
    public Guid ArchivoId { get; set; }
    /// <summary>Columna tipo_documento (text); obligatoria.</summary>
    public string TipoDocumento { get; set; } = string.Empty;
    /// <summary>Relación hacia verificaciones mediante verificacion_id.</summary>
    [JsonIgnore]
    public Verificacion Verificacion { get; set; } = null!;
    /// <summary>Relación hacia archivos mediante archivo_id.</summary>
    [JsonIgnore]
    public Archivo Archivo { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(VerificacionId != Guid.Empty, "VerificacionId es obligatorio.");
        Reglas.Exigir(ArchivoId != Guid.Empty, "ArchivoId es obligatorio.");
        Reglas.Texto(TipoDocumento, nameof(TipoDocumento), 64, true, false);
    }
}
