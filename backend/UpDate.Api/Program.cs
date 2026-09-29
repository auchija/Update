using UpDate.Api.Configuration;
using UpDate.Api.Database;
using UpDate.Api.Errors;
using UpDate.Api.Health;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFrontendCors(builder.Configuration);
builder.Services.AddDatabase();
builder.Services.AddApiHealthChecks();

var app = builder.Build();

app.UseApiErrorHandling();
app.UseCors(CorsSetup.FrontendPolicy);

app.MapApiHealthChecks();
app.MapApiNotFoundFallback();

app.Run();
