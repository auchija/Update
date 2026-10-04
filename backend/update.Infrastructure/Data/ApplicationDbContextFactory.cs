using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Npgsql;
using System;
using System.IO;

namespace update.Infrastructure.Data;

public sealed class ApplicationDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        string? connectionString = null;

        var startupProjectPath = Path.GetFullPath(
            Path.Combine(Directory.GetCurrentDirectory(), "..", "update.API"));

        var appSettingsPath = Path.Combine(
            startupProjectPath,
            "appsettings.json");

        if (File.Exists(appSettingsPath))
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(startupProjectPath)
                    .AddJsonFile(
                        "appsettings.json",
                        optional: true,
                        reloadOnChange: false)
                    .Build();

                connectionString =
                    configuration.GetConnectionString("DefaultConnection");
            }
            catch (IOException)
            {
                // Se intentará usar una variable de entorno.
            }
            catch (FormatException)
            {
                // JSON inválido; se intentará usar una variable de entorno.
            }
        }

        var environmentConnectionString =
            Environment.GetEnvironmentVariable(
                "ConnectionStrings__DefaultConnection")
            ?? Environment.GetEnvironmentVariable(
                "POSTGRES_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = environmentConnectionString;
        }

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "No se pudo determinar la cadena de conexión. " +
                "Configura ConnectionStrings:DefaultConnection en " +
                "appsettings.json o usa las variables de entorno " +
                "ConnectionStrings__DefaultConnection o " +
                "POSTGRES_CONNECTION_STRING.");
        }

        var builder = new NpgsqlConnectionStringBuilder(connectionString);

        if (string.IsNullOrWhiteSpace(builder.Host))
        {
            throw new InvalidOperationException(
                "Falta el host en la cadena de conexión.");
        }

        var finalConnectionString = builder.ConnectionString;

        // No muestres la cadena completa: podría contener la contraseña.
        Console.WriteLine(
            $"Conexión configurada para el host: {builder.Host}");

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(finalConnectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}

