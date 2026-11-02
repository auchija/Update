using Microsoft.OpenApi;
using update.API.Extensions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using update.API.Converters;
using update.API.Errors;
using update.Application.Services;
using update.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var cadena = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Configura ConnectionStrings:DefaultConnection.");
var claveJwt = builder.Configuration["Jwt:Clave"];
if (string.IsNullOrWhiteSpace(claveJwt) || Encoding.UTF8.GetByteCount(claveJwt) < 32)
    throw new InvalidOperationException("Configura Jwt:Clave con al menos 32 bytes mediante user-secrets o una variable de entorno.");

builder.Services.AgregarInfraestructura(cadena);
builder.Services.AddScoped(typeof(ServicioCrud<>));
builder.Services.AddScoped<ServicioMiPerfil>();
var carpetaUploads = Path.Combine(builder.Environment.ContentRootPath, "uploads");
Directory.CreateDirectory(carpetaUploads);
builder.Services.AddSingleton(new OpcionesSubida(carpetaUploads));
builder.Services.AddScoped<ServicioSubidaArchivos>();
builder.Services.AddControllers(opciones =>
    // Los errores de validación usan el nombre del campo en el JSON ("password"), no el de C# ("Password").
    opciones.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider()))
.AddJsonOptions(opciones =>
{
    opciones.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    opciones.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower, allowIntegerValues: false));
    opciones.JsonSerializerOptions.Converters.Add(new DireccionIpConverter());
})
.ConfigureApiBehaviorOptions(opciones => opciones.InvalidModelStateResponseFactory = RespuestaValidacion.Crear);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ManejadorExcepciones>();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT administrativo. Pega únicamente el token en Authorize."
    });
    opciones.AddSecurityRequirement(documento => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", documento)] = []
    });
});
builder.Services.AddApplicationHealthChecks(builder.Configuration);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opciones =>
{
    opciones.MapInboundClaims = false;
    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Emisor"] ?? "UpDate",
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audiencia"] ?? "UpDate.API",
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveJwt)),
        RoleClaimType = "rol",
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});
// Este CRUD es administrativo. Los casos de uso de usuario final necesitan reglas de propiedad.
builder.Services.AddAuthorization(opciones => opciones.AddPolicy("Administracion",
    politica => politica.RequireAuthenticatedUser().RequireRole("administrador")));
var origenes = builder.Configuration.GetSection("Cors:Origenes").Get<string[]>() ?? ["http://localhost:5173"];
builder.Services.AddCors(opciones => opciones.AddPolicy("Frontend",
    politica => politica.WithOrigins(origenes).AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

var app = builder.Build();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(carpetaUploads),
    RequestPath = "/uploads",
    OnPrepareResponse = contexto => contexto.Context.Response.Headers.XContentTypeOptions = "nosniff"
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapApplicationHealthChecks();
app.MapOpenApi().RequireAuthorization("Administracion");
// Las migraciones se ejecutan mediante dotnet ef; no se modifica la base al iniciar la API.
app.Run();
