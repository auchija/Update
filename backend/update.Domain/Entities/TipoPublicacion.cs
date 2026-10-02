using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla tipos_publicacion.</summary>
public sealed class TipoPublicacion : IEntidadValidable
{
    /// <summary>Columna codigo (text); obligatoria.</summary>
    public string Codigo { get; set; } = string.Empty;
    /// <summary>Columna nombre (text); obligatoria.</summary>
    public string Nombre { get; set; } = string.Empty;
    /// <summary>Columna descripcion (text); opcional.</summary>
    public string? Descripcion { get; set; }
    /// <summary>Columna icono (text); opcional.</summary>
    public string? Icono { get; set; }
    /// <summary>Columna peso_descubrimiento (numeric(4,2)); obligatoria.</summary>
    public decimal PesoDescubrimiento { get; set; } = 1.00m;
    /// <summary>Columna permite_respuesta_aceptada (boolean); obligatoria.</summary>
    public bool PermiteRespuestaAceptada { get; set; } = false;
    /// <summary>Columna orden (smallint); obligatoria.</summary>
    public short Orden { get; set; } = (short)0;
    /// <summary>Columna activo (boolean); obligatoria.</summary>
    public bool Activo { get; set; } = true;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Texto(Codigo, nameof(Codigo), 64, true, false);
        Reglas.Texto(Nombre, nameof(Nombre), 255, true, false);
        Reglas.Texto(Descripcion, nameof(Descripcion), 10000, false, false);
        Reglas.Texto(Icono, nameof(Icono), 255, false, false);
    }
}
