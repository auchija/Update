using Microsoft.AspNetCore.Mvc;

namespace update.API.Errors;

/// <summary>
/// Respuesta 400 con el mismo formato que los 409: un mensaje general y un mensaje por campo
/// ({ "password": "..." }), que es lo que el frontend pinta debajo de cada input.
/// </summary>
public static class RespuestaValidacion
{
    public static IActionResult Crear(ActionContext contexto)
    {
        var errores = new Dictionary<string, string>();
        foreach (var (clave, entrada) in contexto.ModelState)
        {
            var mensaje = entrada.Errors.FirstOrDefault()?.ErrorMessage;
            var campo = NombreCampo(clave);
            if (string.IsNullOrWhiteSpace(mensaje) || campo is null) continue;
            errores.TryAdd(campo, mensaje);
        }

        var problema = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Revisa los datos e inténtalo de nuevo.",
            Instance = contexto.HttpContext.Request.Path,
            Extensions =
            {
                ["message"] = errores.Count > 0
                    ? "Revisa los datos marcados."
                    : "La petición no tiene el formato esperado.",
                ["errors"] = errores
            }
        };
        return new BadRequestObjectResult(problema) { ContentTypes = { "application/problem+json" } };
    }

    // Las claves llegan como "password" o, si falla la lectura del JSON, como "$.password" o "$".
    private static string? NombreCampo(string clave)
    {
        var limpia = clave.StartsWith("$.") ? clave[2..] : clave;
        if (limpia.Length == 0 || limpia.StartsWith('$')) return null;
        return char.ToLowerInvariant(limpia[0]) + limpia[1..];
    }
}
