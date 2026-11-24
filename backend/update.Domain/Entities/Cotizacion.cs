using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Cotizacion : BaseEntity, IEntidadValidable
{
    public Guid SolicitudCotizacionId { get; set; }
    public short Version { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public string MonedaCodigo { get; set; } = "COP";
    public decimal Subtotal { get; set; } = 0m;
    public decimal Descuento { get; set; } = 0m;
    public decimal Impuesto { get; set; } = 0m;
    public decimal? Total { get; set; }
    public DateOnly? ValidoHasta { get; set; }
    public string? Notas { get; set; }
    public EstadoCotizacion Estado { get; set; } = EstadoCotizacion.Enviado;
    [JsonIgnore]
    public SolicitudCotizacion SolicitudCotizacion { get; set; } = null!;
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    [JsonIgnore]
    public Moneda Moneda { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(SolicitudCotizacionId != Guid.Empty, "SolicitudCotizacionId es obligatorio.");
        Reglas.Exigir(CreadoPorUsuarioId != Guid.Empty, "CreadoPorUsuarioId es obligatorio.");
        Reglas.Texto(MonedaCodigo, nameof(MonedaCodigo), 3, true, false);
        Reglas.Exigir(MonedaCodigo is null || MonedaCodigo.Length == 3, "MonedaCodigo debe tener 3 caracteres.");
        Reglas.Exigir(Subtotal >= 0, "Subtotal no puede ser negativo.");
        Reglas.Exigir(Descuento >= 0, "Descuento no puede ser negativo.");
        Reglas.Exigir(Impuesto >= 0, "Impuesto no puede ser negativo.");
        Reglas.Texto(Notas, nameof(Notas), 4000, false, false);
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }
}
