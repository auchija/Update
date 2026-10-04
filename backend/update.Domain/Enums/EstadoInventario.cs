namespace update.Domain.Enums;

/// <summary>Valores permitidos para estado_inventario.</summary>
public enum EstadoInventario
{
    /// <summary>Valor del modelo: disponible.</summary>
    Disponible = 0,
    /// <summary>Valor del modelo: agotado.</summary>
    Agotado = 1,
    /// <summary>Valor del modelo: bajo pedido.</summary>
    BajoPedido = 2,
    /// <summary>Valor del modelo: no aplica.</summary>
    NoAplica = 3,
}
