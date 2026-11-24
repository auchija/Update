namespace update.Domain.Exceptions;

public sealed class RecursoNoEncontradoException(string mensaje) : Exception(mensaje);

public sealed class AccesoDenegadoException(string mensaje) : Exception(mensaje);

public sealed class ConflictoException(string mensaje) : Exception(mensaje);

/// <summary>Conflicto con errores por campo, para que el formulario marque cuál corregir.</summary>
public sealed class DatosEnConflictoException(string titulo, string mensaje, IReadOnlyDictionary<string, string> errores)
    : Exception(mensaje)
{
    public string Titulo { get; } = titulo;
    public IReadOnlyDictionary<string, string> Errores { get; } = errores;
}
