using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class PublicacionArchivo : IEntidadValidable
{
    public Guid PublicacionId { get; set; }
    public Guid ArchivoId { get; set; }
    public short Posicion { get; set; }
    public string? TextoAlternativo { get; set; }
    [JsonIgnore]
    public Archivo Archivo { get; set; } = null!;
    [JsonIgnore]
    public Publicacion Publicacion { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(PublicacionId != Guid.Empty, "PublicacionId es obligatorio.");
        Reglas.Exigir(ArchivoId != Guid.Empty, "ArchivoId es obligatorio.");
        Reglas.Texto(TextoAlternativo, nameof(TextoAlternativo), 255, false, false);
    }
}
