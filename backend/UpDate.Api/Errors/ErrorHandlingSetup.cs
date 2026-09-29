namespace UpDate.Api.Errors;

public static class ErrorHandlingSetup
{
    private const string NotFoundMessage = "No encontramos lo que buscabas.";
    private const string ServerErrorMessage = "Algo falló en el servidor. Inténtalo de nuevo más tarde.";

    public static WebApplication UseApiErrorHandling(this WebApplication app)
    {
        app.UseExceptionHandler(errorApp => errorApp.Run(context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return context.Response.WriteAsJsonAsync(new ErrorResponse(ServerErrorMessage));
        }));

        return app;
    }

    public static IEndpointRouteBuilder MapApiNotFoundFallback(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapFallback(() => Results.NotFound(new ErrorResponse(NotFoundMessage)));
        return endpoints;
    }
}
