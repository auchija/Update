using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace UpDate.Api.Tests;

public sealed class ApiFactory(string connectionString) : WebApplicationFactory<Program>
{
    public const string UnreachableDatabase =
        "Host=127.0.0.1;Port=1;Database=update;Username=update;Password=update";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configuration) =>
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Database"] = connectionString,
            }));
    }
}
