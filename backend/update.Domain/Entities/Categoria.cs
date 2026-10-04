using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla categorias.</summary>
public sealed class Categoria : IEntidadValidable
{
    /// <summary>Columna id (integer); obligatoria.</summary>
    public int Id { get; set; }
    /// <summary>Columna padre_id (integer); opcional.</summary>
    public int? PadreId { get; set; }
    /// <summary>Columna slug (text); obligatoria.</summary>
    public string Slug { get; set; } = string.Empty;
    /// <summary>Columna nombre (text); obligatoria.</summary>
    public string Nombre { get; set; } = string.Empty;
    /// <summary>Columna icono (text); opcional.</summary>
    public string? Icono { get; set; }
    /// <summary>Columna orden (smallint); obligatoria.</summary>
    public short Orden { get; set; } = (short)0;
    /// <summary>Columna activo (boolean); obligatoria.</summary>
    public bool Activo { get; set; } = true;
    /// <summary>Relación hacia categorias mediante padre_id.</summary>
    [JsonIgnore]
    public Categoria? Padre { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Texto(Slug, nameof(Slug), 255, true, false);
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
        Reglas.Texto(Icono, nameof(Icono), 255, false, false);
    }
}
