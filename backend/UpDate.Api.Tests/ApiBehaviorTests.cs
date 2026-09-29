using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using UpDate.Api.Errors;
using UpDate.Api.Health;

namespace UpDate.Api.Tests;

public sealed class ApiBehaviorTests
{
    private const string FrontendOrigin = "http://localhost:5173";

    [Fact]
    public async Task UnknownRouteReturnsNotFoundWithMessage()
    {
        await using var factory = new ApiFactory(ApiFactory.UnreachableDatabase);
        using var client = factory.CreateClient();

        using var response = await client.GetAsync(
            "/api/ruta-que-no-existe",
            TestContext.Current.CancellationToken);
        var body = await response.Content.ReadFromJsonAsync<ErrorResponse>(
            TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("No encontramos lo que buscabas.", body?.Message);
    }

    [Fact]
    public async Task AllowsRequestsWithCredentialsFromFrontend()
    {
        await using var factory = new ApiFactory(ApiFactory.UnreachableDatabase);
        using var client = factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, HealthSetup.HealthPath);
        request.Headers.Add("Origin", FrontendOrigin);

        using var response = await client.SendAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal(FrontendOrigin, response.Headers.GetValues("Access-Control-Allow-Origin").Single());
        Assert.Equal("true", response.Headers.GetValues("Access-Control-Allow-Credentials").Single());
    }

    [Fact]
    public async Task FailsToStartWithoutConnectionString()
    {
        await using var factory = new ApiFactory(string.Empty);

        var exception = Assert.Throws<OptionsValidationException>(() => factory.CreateClient());

        Assert.Contains("ConnectionStrings:Database", exception.Message, StringComparison.Ordinal);
    }
}
