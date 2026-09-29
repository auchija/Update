using System.Net;
using System.Net.Http.Json;
using UpDate.Api.Health;

namespace UpDate.Api.Tests;

public sealed class HealthEndpointTests
{
    private const string TestDatabaseVariable = "UPDATE_TEST_DATABASE";

    [Fact]
    public async Task ReturnsServiceUnavailableWhenDatabaseIsUnreachable()
    {
        await using var factory = new ApiFactory(ApiFactory.UnreachableDatabase);
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(
            HealthSetup.HealthPath,
            TestContext.Current.CancellationToken);
        var body = await response.Content.ReadFromJsonAsync<HealthResponse>(
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.Equal("error", body?.Status);
        Assert.Equal("error", body?.Database.Status);
        Assert.Equal("No se pudo conectar con la base de datos.", body?.Message);
    }

    [Fact]
    public async Task ReturnsDatabaseStatusWhenDatabaseIsReachable()
    {
        var connectionString = Environment.GetEnvironmentVariable(TestDatabaseVariable);
        Assert.SkipWhen(
            string.IsNullOrWhiteSpace(connectionString),
            $"Define {TestDatabaseVariable} con una base de datos de prueba para ejecutar esta prueba.");

        await using var factory = new ApiFactory(connectionString!);
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(
            HealthSetup.HealthPath,
            TestContext.Current.CancellationToken);
        var body = await response.Content.ReadFromJsonAsync<HealthResponse>(
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("ok", body?.Status);
        Assert.Equal("ok", body?.Database.Status);
        Assert.Equal(56, body?.Database.Tables);
        Assert.False(string.IsNullOrEmpty(body?.Database.Version));
    }
}
