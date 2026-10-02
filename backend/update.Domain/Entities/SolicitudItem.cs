using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla solicitud_items.</summary>
public sealed class SolicitudItem : BaseEntity, IEntidadValidable
{
    /// <summary>Columna solicitud_cotizacion_id (uuid); obligatoria.</summary>
    public Guid SolicitudCotizacionId { get; set; }
    /// <summary>Columna producto_id (uuid); opcional.</summary>
    public Guid? ProductoId { get; set; }
    /// <summary>Columna descripcion (text); obligatoria.</summary>
    public string Descripcion { get; set; } = string.Empty;
    /// <summary>Columna cantidad (numeric(12,2)); obligatoria.</summary>
    public decimal Cantidad { get; set; } = 1m;
    /// <summary>Columna notas (text); opcional.</summary>
    public string? Notas { get; set; }
    /// <summary>Relación hacia solicitudes_cotizacion mediante solicitud_cotizacion_id.</summary>
    [JsonIgnore]
    public SolicitudCotizacion SolicitudCotizacion { get; set; } = null!;
    /// <summary>Relación hacia productos mediante producto_id.</summary>
    [JsonIgnore]
    public Producto? Producto { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(SolicitudCotizacionId != Guid.Empty, "SolicitudCotizacionId es obligatorio.");
        Reglas.Texto(Descripcion, nameof(Descripcion), 10000, true, false);
        Reglas.Exigir(Cantidad >= 0, "Cantidad no puede ser negativo.");
        Reglas.Texto(Notas, nameof(Notas), 4000, false, false);
    }
}
