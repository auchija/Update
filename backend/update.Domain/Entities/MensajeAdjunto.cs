using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class MensajeAdjunto : IEntidadValidable
{
    public Guid MensajeId { get; set; }
    public Guid ArchivoId { get; set; }
    public short Posicion { get; set; }
    [JsonIgnore]
    public Mensaje Mensaje { get; set; } = null!;
    [JsonIgnore]
    public Archivo Archivo { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(MensajeId != Guid.Empty, "MensajeId es obligatorio.");
        Reglas.Exigir(ArchivoId != Guid.Empty, "ArchivoId es obligatorio.");
    }
}
