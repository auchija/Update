using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class DocumentoVerificacion : IEntidadValidable
{
    public Guid VerificacionId { get; set; }
    public Guid ArchivoId { get; set; }
    public string TipoDocumento { get; set; } = string.Empty;
    [JsonIgnore]
    public Verificacion Verificacion { get; set; } = null!;
    [JsonIgnore]
    public Archivo Archivo { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(VerificacionId != Guid.Empty, "VerificacionId es obligatorio.");
        Reglas.Exigir(ArchivoId != Guid.Empty, "ArchivoId es obligatorio.");
        Reglas.Texto(TipoDocumento, nameof(TipoDocumento), 64, true, false);
    }
}
