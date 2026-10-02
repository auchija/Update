using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using update.Domain.Exceptions;
namespace update.API.Errors;

/// <summary>Respuestas ProblemDetails sin divulgar SQL, credenciales o detalles internos.</summary>
public sealed class ManejadorExcepciones(IProblemDetailsService problemas, ILogger<ManejadorExcepciones> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext contexto, Exception excepcion, CancellationToken cancellationToken)
    {
        var estado = StatusCodes.Status500InternalServerError;
        var titulo = "No se pudo completar la operación.";
        if (excepcion is ExcepcionDominio)
        {
            estado = StatusCodes.Status400BadRequest;
            titulo = excepcion.Message;
        }
        else if (excepcion is DbUpdateConcurrencyException)
        {
            estado = StatusCodes.Status409Conflict;
            titulo = "El registro cambió o fue eliminado durante la operación.";
        }
        else if (excepcion is DbUpdateException { InnerException: PostgresException postgres })
        {
            (estado, titulo) = postgres.SqlState switch
            {
                PostgresErrorCodes.UniqueViolation => (409, "Ya existe un registro con esos valores únicos."),
                PostgresErrorCodes.ForeignKeyViolation => (409, "La operación incumple una relación con otro registro."),
                PostgresErrorCodes.CheckViolation => (400, "Los datos incumplen una restricción de la tabla."),
                PostgresErrorCodes.NotNullViolation => (400, "Falta un dato obligatorio."),
                PostgresErrorCodes.StringDataRightTruncation => (400, "Un texto supera la longitud permitida."),
                PostgresErrorCodes.NumericValueOutOfRange => (400, "Un número supera el rango permitido."),
                _ => (500, "No se pudo guardar el registro.")
            };
        }
        if (estado >= 500) logger.LogError(excepcion, "Error en {Ruta}", contexto.Request.Path);
        contexto.Response.StatusCode = estado;
        await problemas.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = contexto,
            ProblemDetails = new ProblemDetails { Status = estado, Title = titulo, Instance = contexto.Request.Path }
        });
        return true;
    }
}
