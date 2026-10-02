using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla monedas.</summary>
public sealed class Moneda : IEntidadValidable
{
    /// <summary>Columna codigo (char(3)); obligatoria.</summary>
    public string Codigo { get; set; } = string.Empty;
    /// <summary>Columna nombre (text); obligatoria.</summary>
    public string Nombre { get; set; } = string.Empty;
    /// <summary>Columna simbolo (text); obligatoria.</summary>
    public string Simbolo { get; set; } = string.Empty;
    /// <summary>Columna decimales (smallint); obligatoria.</summary>
    public short Decimales { get; set; } = (short)0;
    /// <summary>Columna activo (boolean); obligatoria.</summary>
    public bool Activo { get; set; } = true;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Texto(Codigo, nameof(Codigo), 3, true, false);
        Reglas.Exigir(Codigo is null || Codigo.Length == 3, "Codigo debe tener 3 caracteres.");
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
        Reglas.Texto(Simbolo, nameof(Simbolo), 255, true, false);
    }
}
