using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla paises.</summary>
public sealed class Pais : IEntidadValidable
{
    /// <summary>Columna codigo (char(2)); obligatoria.</summary>
    public string Codigo { get; set; } = string.Empty;
    /// <summary>Columna nombre (text); obligatoria.</summary>
    public string Nombre { get; set; } = string.Empty;
    /// <summary>Columna moneda_defecto_codigo (char(3)); obligatoria.</summary>
    public string MonedaDefectoCodigo { get; set; } = string.Empty;
    /// <summary>Columna prefijo_telefono (text); obligatoria.</summary>
    public string PrefijoTelefono { get; set; } = string.Empty;
    /// <summary>Relación hacia monedas mediante moneda_defecto_codigo.</summary>
    [JsonIgnore]
    public Moneda MonedaDefecto { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
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
