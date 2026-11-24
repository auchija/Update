using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class SolicitudItem : BaseEntity, IEntidadValidable
{
    public Guid SolicitudCotizacionId { get; set; }
    public Guid? ProductoId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal Cantidad { get; set; } = 1m;
    public string? Notas { get; set; }
    [JsonIgnore]
    public SolicitudCotizacion SolicitudCotizacion { get; set; } = null!;
    [JsonIgnore]
    public Producto? Producto { get; set; }

    public void Validar()
    {
        Reglas.Exigir(SolicitudCotizacionId != Guid.Empty, "SolicitudCotizacionId es obligatorio.");
        Reglas.Texto(Descripcion, nameof(Descripcion), 10000, true, false);
        Reglas.Exigir(Cantidad >= 0, "Cantidad no puede ser negativo.");
        Reglas.Texto(Notas, nameof(Notas), 4000, false, false);
    }
}
