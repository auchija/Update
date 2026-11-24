using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Etiqueta : IEntidadValidable
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int TotalUsos { get; set; } = 0;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

    public void Validar()
    {
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
        Reglas.Exigir(TotalUsos >= 0, "TotalUsos no puede ser negativo.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }
}
