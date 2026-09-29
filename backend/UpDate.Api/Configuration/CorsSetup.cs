namespace UpDate.Api.Configuration;

public static class CorsSetup
{
    public const string FrontendPolicy = "Frontend";

    private const string DefaultFrontendUrl = "http://localhost:5173";

    public static IServiceCollection AddFrontendCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var frontendUrl = configuration["FrontendUrl"] ?? DefaultFrontendUrl;

        services.AddCors(options => options.AddPolicy(FrontendPolicy, policy => policy
            .WithOrigins(frontendUrl)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()));

        return services;
    }
}
