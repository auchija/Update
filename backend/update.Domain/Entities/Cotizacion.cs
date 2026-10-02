using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla cotizaciones.</summary>
public sealed class Cotizacion : BaseEntity, IEntidadValidable
{
    /// <summary>Columna solicitud_cotizacion_id (uuid); obligatoria.</summary>
    public Guid SolicitudCotizacionId { get; set; }
    /// <summary>Columna version (smallint); obligatoria.</summary>
    public short Version { get; set; }
    /// <summary>Columna creado_por_usuario_id (uuid); obligatoria.</summary>
    public Guid CreadoPorUsuarioId { get; set; }
    /// <summary>Columna moneda_codigo (char(3)); obligatoria.</summary>
    public string MonedaCodigo { get; set; } = "COP";
    /// <summary>Columna subtotal (numeric(14,2)); obligatoria.</summary>
    public decimal Subtotal { get; set; } = 0m;
    /// <summary>Columna descuento (numeric(14,2)); obligatoria.</summary>
    public decimal Descuento { get; set; } = 0m;
    /// <summary>Columna impuesto (numeric(14,2)); obligatoria.</summary>
    public decimal Impuesto { get; set; } = 0m;
    /// <summary>Columna total (numeric(14,2)); opcional.</summary>
    public decimal? Total { get; set; }
    /// <summary>Columna valido_hasta (date); opcional.</summary>
    public DateOnly? ValidoHasta { get; set; }
    /// <summary>Columna notas (text); opcional.</summary>
    public string? Notas { get; set; }
    /// <summary>Columna estado (estado_cotizacion); obligatoria.</summary>
    public EstadoCotizacion Estado { get; set; } = EstadoCotizacion.Enviado;
    /// <summary>Relación hacia solicitudes_cotizacion mediante solicitud_cotizacion_id.</summary>
    [JsonIgnore]
    public SolicitudCotizacion SolicitudCotizacion { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante creado_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;
    /// <summary>Relación hacia monedas mediante moneda_codigo.</summary>
    [JsonIgnore]
    public Moneda Moneda { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
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
