namespace update.Domain.Common;

/// <summary>Identidad UUID y auditoría compartida de las entidades compatibles.</summary>
public abstract class BaseEntity
{
    /// <summary>Identificador; PostgreSQL lo genera salvo en subtipos con clave compartida.</summary>
    public Guid Id { get; set; }
    /// <summary>Fecha de creación UTC; se ignora en EF si la tabla no tiene esta columna.</summary>
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    /// <summary>Fecha de actualización UTC; se ignora si la tabla no tiene esta columna.</summary>
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
}
