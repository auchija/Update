using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class ResenaArchivo : IEntidadValidable
{
    public Guid ResenaId { get; set; }
    public Guid ArchivoId { get; set; }
    public short Posicion { get; set; }
    [JsonIgnore]
    public Resena Resena { get; set; } = null!;
    [JsonIgnore]
    public Archivo Archivo { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(ResenaId != Guid.Empty, "ResenaId es obligatorio.");
        Reglas.Exigir(ArchivoId != Guid.Empty, "ArchivoId es obligatorio.");
    }
}
