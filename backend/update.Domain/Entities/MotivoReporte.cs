using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla motivos_reporte.</summary>
public sealed class MotivoReporte : IEntidadValidable
{
    /// <summary>Columna codigo (text); obligatoria.</summary>
    public string Codigo { get; set; } = string.Empty;
    /// <summary>Columna nombre (text); obligatoria.</summary>
    public string Nombre { get; set; } = string.Empty;
    /// <summary>Columna orden (smallint); obligatoria.</summary>
    public short Orden { get; set; } = (short)0;
    /// <summary>Columna activo (boolean); obligatoria.</summary>
    public bool Activo { get; set; } = true;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Texto(Codigo, nameof(Codigo), 64, true, false);
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
    }
}
