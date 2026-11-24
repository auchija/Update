using Microsoft.OpenApi;
using update.API.Extensions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

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
builder.Services.AddHttpClient("storage").ConfigurePrimaryHttpMessageHandler(
    () => new HttpClientHandler { AllowAutoRedirect = false });
builder.Services.AddScoped<IAlmacenObjetos>(sp =>
{
    var url = builder.Configuration["Supabase:Url"];
    var clave = builder.Configuration["Supabase:ServiceRoleKey"];
    var bucket = builder.Configuration["Supabase:Bucket"];
    var urlValida = Uri.TryCreate(url, UriKind.Absolute, out var baseUri)
        && (baseUri.Scheme == "https"
            || (builder.Environment.IsDevelopment() && baseUri.Scheme == "http" && baseUri.IsLoopback));
    if (!urlValida
        || string.IsNullOrWhiteSpace(clave) || string.IsNullOrWhiteSpace(bucket))
        throw new InvalidOperationException("Configura Supabase:Url, Bucket y ServiceRoleKey en el servidor.");
    var http = sp.GetRequiredService<IHttpClientFactory>().CreateClient("storage");
    http.BaseAddress = new Uri(baseUri!, "/storage/v1/");
    http.DefaultRequestHeaders.Add("apikey", clave);
    http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", clave);
    return new AlmacenSupabase(http, bucket);
});
builder.Services.AddScoped(sp => new Lazy<IAlmacenObjetos>(sp.GetRequiredService<IAlmacenObjetos>));
builder.Services.AddSingleton(new OpcionesSubida(carpetaUploads));
builder.Services.AddScoped<ServicioSubidaArchivos>();
builder.Services.AddScoped<ServicioFeed>();
builder.Services.AddScoped<ServicioComunidad>();
builder.Services.AddScoped<MisEmprendimientos>();
builder.Services.AddControllers(opciones =>
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
    foreach (var proyecto in new[] { "update.API", "update.Application" })
    {
        var xml = Path.Combine(AppContext.BaseDirectory, $"{proyecto}.xml");
        if (File.Exists(xml)) opciones.IncludeXmlComments(xml);
    }
    opciones.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Pega el accessToken que devuelve POST /api/auth/login o /api/auth/register (sin la palabra Bearer)."
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
builder.Services.AddAuthorization(opciones => opciones.AddPolicy("Administracion",
    politica => politica.RequireAuthenticatedUser().RequireRole("administrador")));
var origenes = builder.Configuration.GetSection("Cors:Origenes").Get<string[]>() ?? ["http://localhost:5173"];
builder.Services.AddCors(opciones => opciones.AddPolicy("Frontend",
    politica => politica.WithOrigins(origenes).AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
builder.Services.Configure<ForwardedHeadersOptions>(opciones =>
{
    opciones.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
    opciones.KnownIPNetworks.Clear();
    opciones.KnownProxies.Clear();
});

var app = builder.Build();
app.UseForwardedHeaders();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("Swagger:Habilitado"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment()) app.UseHttpsRedirection();
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapApplicationHealthChecks();
app.MapOpenApi().RequireAuthorization("Administracion");
app.Run();
