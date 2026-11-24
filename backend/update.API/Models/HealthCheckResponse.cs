namespace update.API.Models;

public class HealthCheckResponse
{
    public string Status { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public long DurationMs { get; set; }

    public Dictionary<string, HealthCheckDetail> Checks { get; set; } = new();
}

public class HealthCheckDetail
{
    public string Status { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public long DurationMs { get; set; }

    public Dictionary<string, object>? Data { get; set; }

    public string? Exception { get; set; }
}
