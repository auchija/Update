using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class VarianteArchivo : IEntidadValidable
{
    public Guid ArchivoId { get; set; }
    public string Variante { get; set; } = string.Empty;
    public string ClaveObjeto { get; set; } = string.Empty;
    public string TipoMime { get; set; } = string.Empty;
    public int? Ancho { get; set; }
    public int? Alto { get; set; }
    public long TamanoBytes { get; set; }
    [JsonIgnore]
    public Archivo Archivo { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(ArchivoId != Guid.Empty, "ArchivoId es obligatorio.");
        Reglas.Texto(Variante, nameof(Variante), 64, true, false);
        Reglas.Texto(ClaveObjeto, nameof(ClaveObjeto), 1024, true, false);
        Reglas.Texto(TipoMime, nameof(TipoMime), 127, true, false);
        Reglas.Exigir(TamanoBytes >= 0, "TamanoBytes no puede ser negativo.");
    }
}
