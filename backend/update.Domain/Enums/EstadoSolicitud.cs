namespace update.Domain.Enums;

/// <summary>Valores permitidos para estado_solicitud.</summary>
public enum EstadoSolicitud
{
    /// <summary>Valor del modelo: pendiente.</summary>
    Pendiente = 0,
    /// <summary>Valor del modelo: cotizado.</summary>
    Cotizado = 1,
    /// <summary>Valor del modelo: aceptado.</summary>
    Aceptado = 2,
    /// <summary>Valor del modelo: rechazado.</summary>
    Rechazado = 3,
    /// <summary>Valor del modelo: cancelado.</summary>
    Cancelado = 4,
    /// <summary>Valor del modelo: vencido.</summary>
    Vencido = 5,
    /// <summary>Valor del modelo: completado.</summary>
    Completado = 6,
}
