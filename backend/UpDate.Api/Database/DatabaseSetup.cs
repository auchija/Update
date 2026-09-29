using Microsoft.Extensions.Options;
using Npgsql;

namespace UpDate.Api.Database;

public static class DatabaseSetup
{
    private const int TimeoutSeconds = 5;

    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddOptions<DatabaseOptions>()
            .Configure<IConfiguration>((options, configuration) =>
                options.ConnectionString =
                    configuration.GetConnectionString(DatabaseOptions.ConnectionStringName) ?? string.Empty)
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.ConnectionString),
                "Falta la cadena de conexión ConnectionStrings:Database. Configúrala con dotnet user-secrets o con la variable de entorno ConnectionStrings__Database.")
            .ValidateOnStart();

        services.AddSingleton(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            return NpgsqlDataSource.Create(WithTimeouts(options.ConnectionString));
        });

        return services;
    }

    private static string WithTimeouts(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Timeout = TimeoutSeconds,
            CommandTimeout = TimeoutSeconds,
        };

        return builder.ConnectionString;
    }
}
