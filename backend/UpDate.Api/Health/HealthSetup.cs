using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace UpDate.Api.Health;

public static class HealthSetup
{
    public const string HealthPath = "/api/health";

    private static readonly TimeSpan CheckTimeout = TimeSpan.FromSeconds(5);

    public static IServiceCollection AddApiHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>(DatabaseHealthCheck.Name, timeout: CheckTimeout);

        return services;
    }

    public static IEndpointRouteBuilder MapApiHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks(HealthPath, new HealthCheckOptions
        {
            ResponseWriter = HealthResponseWriter.WriteAsync,
        });

        return endpoints;
    }
}
