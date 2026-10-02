using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla departamentos.</summary>
public sealed class Departamento : IEntidadValidable
{
    /// <summary>Columna id (integer); obligatoria.</summary>
    public int Id { get; set; }
    /// <summary>Columna pais_codigo (char(2)); obligatoria.</summary>
    public string PaisCodigo { get; set; } = string.Empty;
    /// <summary>Columna nombre (text); obligatoria.</summary>
    public string Nombre { get; set; } = string.Empty;
    /// <summary>Columna codigo (text); opcional.</summary>
    public string? Codigo { get; set; }
    /// <summary>Relación hacia paises mediante pais_codigo.</summary>
    [JsonIgnore]
    public Pais Pais { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Texto(PaisCodigo, nameof(PaisCodigo), 2, true, false);
        Reglas.Exigir(PaisCodigo is null || PaisCodigo.Length == 2, "PaisCodigo debe tener 2 caracteres.");
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
        Reglas.Texto(Codigo, nameof(Codigo), 64, false, false);
    }
}
