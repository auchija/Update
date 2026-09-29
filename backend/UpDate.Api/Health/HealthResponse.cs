namespace UpDate.Api.Health;

public sealed record HealthResponse(
    string Status,
    string? Message,
    DatabaseHealth Database,
    long UptimeSeconds);

public sealed record DatabaseHealth(
    string Status,
    long? LatencyMs = null,
    string? Version = null,
    int? Tables = null,
    DateTimeOffset? ServerTime = null);
