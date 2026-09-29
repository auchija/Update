using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UpDate.Api.Health;

public static class HealthResponseWriter
{
    private const string HealthyStatus = "ok";
    private const string UnhealthyStatus = "error";
    private const string UnhealthyMessage = "No se pudo conectar con la base de datos.";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public static Task WriteAsync(HttpContext context, HealthReport report)
    {
        var response = report.Status == HealthStatus.Healthy
            ? BuildHealthyResponse(report.Entries[DatabaseHealthCheck.Name])
            : BuildUnhealthyResponse();

        return context.Response.WriteAsJsonAsync(response, JsonOptions);
    }

    private static HealthResponse BuildHealthyResponse(HealthReportEntry database)
    {
        var databaseHealth = new DatabaseHealth(
            Status: HealthyStatus,
            LatencyMs: (long)database.Duration.TotalMilliseconds,
            Version: (string)database.Data[HealthDataKeys.Version],
            Tables: (int)database.Data[HealthDataKeys.Tables],
            ServerTime: (DateTimeOffset)database.Data[HealthDataKeys.ServerTime]);

        return new HealthResponse(HealthyStatus, null, databaseHealth, GetUptimeSeconds());
    }

    private static HealthResponse BuildUnhealthyResponse() =>
        new(UnhealthyStatus, UnhealthyMessage, new DatabaseHealth(UnhealthyStatus), GetUptimeSeconds());

    private static long GetUptimeSeconds()
    {
        using var process = Process.GetCurrentProcess();
        return (long)(DateTime.Now - process.StartTime).TotalSeconds;
    }
}
