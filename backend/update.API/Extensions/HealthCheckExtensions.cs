using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;
using System.Net.Mime;
using System.Text.Json;
using update.API.Models;
using update.Infrastructure.Data;

namespace update.API.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddApplicationHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found.");

        services
            .AddHealthChecks()
            .AddCheck(
                name: "aplicacion",
                check: () => HealthCheckResult.Healthy(
                    "Aplicación funcionando correctamente"),
                tags: new[] { "live" })

            .AddDbContextCheck<ApplicationDbContext>(
                name: "base_datos",
                tags: new[] { "db", "postgresql", "ready" })

            .AddNpgSql(
                connectionString: connectionString,
                name: "postgresql",
                tags: new[] { "db", "postgresql", "ready" });

        return services;
    }

    public static WebApplication MapApplicationHealthChecks(
        this WebApplication app)
    {
        app.MapGet("/health", async (
            HealthCheckService healthCheckService,
            HttpContext context) =>
        {
            var stopwatch = Stopwatch.StartNew();

            var report = await healthCheckService.CheckHealthAsync();

            stopwatch.Stop();

            var response = new HealthCheckResponse
            {
                Status = report.Status.ToString(),
                Timestamp = DateTime.UtcNow,
                DurationMs = stopwatch.ElapsedMilliseconds,
                Checks = report.Entries.ToDictionary(
                    entry => entry.Key,
                    entry => new HealthCheckDetail
                    {
                        Status = entry.Value.Status.ToString(),
                        Description = entry.Value.Description ?? string.Empty,
                        DurationMs =
                            (long)entry.Value.Duration.TotalMilliseconds,
                        Data = entry.Value.Data.Count > 0
                            ? new Dictionary<string, object>(
                                entry.Value.Data)
                            : null,
                        Exception = entry.Value.Exception?.Message
                    })
            };

            context.Response.StatusCode =
                report.Status == HealthStatus.Healthy
                    ? StatusCodes.Status200OK
                    : StatusCodes.Status503ServiceUnavailable;

            return Results.Json(response);
        })
        .WithName("Health Check")
        .Produces<HealthCheckResponse>(StatusCodes.Status200OK)
        .Produces<HealthCheckResponse>(
            StatusCodes.Status503ServiceUnavailable);

        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("live"),
            ResponseWriter = WriteHealthResponse
        })
        .WithName("Health Check - Live");

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready"),
            ResponseWriter = WriteHealthResponse
        })
        .WithName("Health Check - Ready");

        return app;
    }

    private static Task WriteHealthResponse(
        HttpContext context,
        HealthReport report)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;

        context.Response.StatusCode =
            report.Status == HealthStatus.Healthy
                ? StatusCodes.Status200OK
                : StatusCodes.Status503ServiceUnavailable;

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var response = new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            checks = report.Entries.ToDictionary(
                entry => entry.Key,
                entry => new
                {
                    status = entry.Value.Status.ToString(),
                    description = entry.Value.Description,
                    duration = entry.Value.Duration.TotalMilliseconds,
                    data = entry.Value.Data.Count > 0
                        ? entry.Value.Data
                        : null,
                    exception = entry.Value.Exception?.Message
                })
        };

        return context.Response.WriteAsJsonAsync(response, options);
    }
}
