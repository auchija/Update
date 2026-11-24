using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class CotizacionItem : BaseEntity, IEntidadValidable
{
    public Guid CotizacionId { get; set; }
    public Guid? ProductoId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal? TotalLinea { get; set; }
    [JsonIgnore]
    public Cotizacion Cotizacion { get; set; } = null!;
    [JsonIgnore]
    public Producto? Producto { get; set; }

    public void Validar()
    {
        Reglas.Exigir(CotizacionId != Guid.Empty, "CotizacionId es obligatorio.");
        Reglas.Texto(Descripcion, nameof(Descripcion), 10000, true, false);
        Reglas.Exigir(Cantidad >= 0, "Cantidad no puede ser negativo.");
        Reglas.Exigir(PrecioUnitario >= 0, "PrecioUnitario no puede ser negativo.");
        Reglas.Exigir(TotalLinea is null || TotalLinea >= 0, "TotalLinea no puede ser negativo.");
    }
}
