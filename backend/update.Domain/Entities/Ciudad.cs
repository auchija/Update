using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla ciudades.</summary>
public sealed class Ciudad : IEntidadValidable
{
    /// <summary>Columna id (integer); obligatoria.</summary>
    public int Id { get; set; }
    /// <summary>Columna departamento_id (integer); obligatoria.</summary>
    public int DepartamentoId { get; set; }
    /// <summary>Columna nombre (text); obligatoria.</summary>
    public string Nombre { get; set; } = string.Empty;
    /// <summary>Columna codigo (text); opcional.</summary>
    public string? Codigo { get; set; }
    /// <summary>Columna latitud (numeric(9,6)); obligatoria.</summary>
    public decimal Latitud { get; set; }
    /// <summary>Columna longitud (numeric(9,6)); obligatoria.</summary>
    public decimal Longitud { get; set; }
    /// <summary>Relación hacia departamentos mediante departamento_id.</summary>
    [JsonIgnore]
    public Departamento Departamento { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(DepartamentoId > 0, "DepartamentoId debe ser positivo.");
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
        Reglas.Texto(Codigo, nameof(Codigo), 64, false, false);
        Reglas.Exigir(Latitud is >= -90m and <= 90m && Longitud is >= -180m and <= 180m, "Las coordenadas están fuera de rango.");
    }
}
