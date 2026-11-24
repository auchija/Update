using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Pais : IEntidadValidable
{
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string MonedaDefectoCodigo { get; set; } = string.Empty;
    public string PrefijoTelefono { get; set; } = string.Empty;
    [JsonIgnore]
    public Moneda MonedaDefecto { get; set; } = null!;

    public void Validar()
    {
        Reglas.Texto(Codigo, nameof(Codigo), 2, true, false);
        Reglas.Exigir(Codigo is null || Codigo.Length == 2, "Codigo debe tener 2 caracteres.");
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
        Reglas.Texto(MonedaDefectoCodigo, nameof(MonedaDefectoCodigo), 3, true, false);
        Reglas.Exigir(MonedaDefectoCodigo is null || MonedaDefectoCodigo.Length == 3, "MonedaDefectoCodigo debe tener 3 caracteres.");
        Reglas.Texto(PrefijoTelefono, nameof(PrefijoTelefono), 30, true, false);
    }
}
