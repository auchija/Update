namespace update.API.Models;

public class HealthCheckResponse
{
    /// <summary>
    /// Estado general: Healthy, Degraded, Unhealthy
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp de la verificación
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Duración total de la verificación en ms
    /// </summary>
    public long DurationMs { get; set; }

    /// <summary>
    /// Detalles de cada health check
    /// </summary>
    public Dictionary<string, HealthCheckDetail> Checks { get; set; } = new();
}

public class HealthCheckDetail
{
    /// <summary>
    /// Estado individual del chequeo
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Descripción del estado
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Duración en ms
    /// </summary>
    public long DurationMs { get; set; }

    /// <summary>
    /// Datos adicionales
    /// </summary>
    public Dictionary<string, object>? Data { get; set; }

    /// <summary>
    /// Mensaje de excepción si ocurrió un error
    /// </summary>
    public string? Exception { get; set; }
}
