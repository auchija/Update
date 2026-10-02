using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla cotizacion_items.</summary>
public sealed class CotizacionItem : BaseEntity, IEntidadValidable
{
    /// <summary>Columna cotizacion_id (uuid); obligatoria.</summary>
    public Guid CotizacionId { get; set; }
    /// <summary>Columna producto_id (uuid); opcional.</summary>
    public Guid? ProductoId { get; set; }
    /// <summary>Columna descripcion (text); obligatoria.</summary>
    public string Descripcion { get; set; } = string.Empty;
    /// <summary>Columna cantidad (numeric(12,2)); obligatoria.</summary>
    public decimal Cantidad { get; set; }
    /// <summary>Columna precio_unitario (numeric(14,2)); obligatoria.</summary>
    public decimal PrecioUnitario { get; set; }
    /// <summary>Columna total_linea (numeric(14,2)); opcional.</summary>
    public decimal? TotalLinea { get; set; }
    /// <summary>Relación hacia cotizaciones mediante cotizacion_id.</summary>
    [JsonIgnore]
    public Cotizacion Cotizacion { get; set; } = null!;
    /// <summary>Relación hacia productos mediante producto_id.</summary>
    [JsonIgnore]
    public Producto? Producto { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(CotizacionId != Guid.Empty, "CotizacionId es obligatorio.");
        Reglas.Texto(Descripcion, nameof(Descripcion), 10000, true, false);
        Reglas.Exigir(Cantidad >= 0, "Cantidad no puede ser negativo.");
        Reglas.Exigir(PrecioUnitario >= 0, "PrecioUnitario no puede ser negativo.");
        Reglas.Exigir(TotalLinea is null || TotalLinea >= 0, "TotalLinea no puede ser negativo.");
    }
}
