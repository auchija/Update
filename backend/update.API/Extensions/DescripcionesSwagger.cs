using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace update.API.Extensions;

internal sealed class DescripcionesSwagger : IOperationFilter
{
    public void Apply(OpenApiOperation operacion, OperationFilterContext contexto)
    {
        var descripcion = contexto.ApiDescription;
        var metadatos = descripcion.ActionDescriptor.EndpointMetadata;

        if (string.IsNullOrWhiteSpace(operacion.Summary))
            operacion.Summary = metadatos.OfType<IEndpointSummaryMetadata>().LastOrDefault()?.Summary
                ?? ResumenCrud(descripcion.HttpMethod, descripcion.RelativePath);

        if (metadatos.OfType<IAllowAnonymous>().Any()) return;
        var autorizacion = metadatos.OfType<IAuthorizeData>().ToList();
        if (autorizacion.Count == 0) return;

        operacion.Responses ??= new OpenApiResponses();
        operacion.Responses.TryAdd("401", new OpenApiResponse { Description = "Falta la sesión o el token venció." });
        if (autorizacion.Any(a => a.Policy is not null))
            operacion.Responses.TryAdd("403", new OpenApiResponse { Description = "Solo para administradores." });
    }

    private static string ResumenCrud(string? metodo, string? ruta)
    {
        var partes = (ruta ?? "").Split('/', StringSplitOptions.RemoveEmptyEntries);
        var recurso = partes.Length > 1 ? partes[1].Replace('-', ' ').Replace('_', ' ') : "registros";
        var porClave = partes.Skip(2).Any(p => p.StartsWith('{'));
        var accion = metodo?.ToUpperInvariant() switch
        {
            "GET" when porClave => "Obtiene un registro de",
            "GET" => "Lista con paginación los registros de",
            "POST" => "Crea un registro en",
            "PUT" => "Actualiza un registro de",
            "DELETE" => "Elimina un registro de",
            _ => "Opera sobre"
        };
        return $"{accion} {recurso} (administración).";
    }
}
