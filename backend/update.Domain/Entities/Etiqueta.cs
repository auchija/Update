using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla etiquetas.</summary>
public sealed class Etiqueta : IEntidadValidable
{
    /// <summary>Columna id (integer); obligatoria.</summary>
    public int Id { get; set; }
    /// <summary>Columna nombre (text); obligatoria.</summary>
    public string Nombre { get; set; } = string.Empty;
    /// <summary>Columna total_usos (integer); obligatoria.</summary>
    public int TotalUsos { get; set; } = 0;
    /// <summary>Columna creado_en (timestamptz); obligatoria.</summary>
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
        Reglas.Exigir(TotalUsos >= 0, "TotalUsos no puede ser negativo.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }
}
