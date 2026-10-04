namespace update.Domain.Enums;

/// <summary>Valores permitidos para estado_verificacion.</summary>
public enum EstadoVerificacion
{
    /// <summary>Valor del modelo: sin verificar.</summary>
    SinVerificar = 0,
    /// <summary>Valor del modelo: pendiente.</summary>
    Pendiente = 1,
    /// <summary>Valor del modelo: aprobado.</summary>
    Aprobado = 2,
    /// <summary>Valor del modelo: rechazado.</summary>
    Rechazado = 3,
}
