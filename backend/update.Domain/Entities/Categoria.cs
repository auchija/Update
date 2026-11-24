using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Categoria : IEntidadValidable
{
    public int Id { get; set; }
    public int? PadreId { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Icono { get; set; }
    public short Orden { get; set; } = (short)0;
    public bool Activo { get; set; } = true;
    [JsonIgnore]
    public Categoria? Padre { get; set; }

    public void Validar()
    {
        Reglas.Texto(Slug, nameof(Slug), 255, true, false);
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
        Reglas.Texto(Icono, nameof(Icono), 255, false, false);
    }
}
