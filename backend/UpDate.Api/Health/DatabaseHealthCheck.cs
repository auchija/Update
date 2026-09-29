using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace UpDate.Api.Health;

public sealed class DatabaseHealthCheck(NpgsqlDataSource dataSource) : IHealthCheck
{
    public const string Name = "database";

    private const string StatusQuery = """
        SELECT current_setting('server_version') AS version,
               (SELECT count(*)::int
                  FROM information_schema.tables
                 WHERE table_schema = 'public') AS table_count,
               now() AS server_time
        """;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        await using var command = dataSource.CreateCommand(StatusQuery);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        await reader.ReadAsync(cancellationToken);

        var status = new Dictionary<string, object>
        {
            [HealthDataKeys.Version] = reader.GetString(0),
            [HealthDataKeys.Tables] = reader.GetInt32(1),
            [HealthDataKeys.ServerTime] = reader.GetFieldValue<DateTimeOffset>(2),
        };

        return HealthCheckResult.Healthy(data: status);
    }
}
