namespace update.Domain.Exceptions;

/// <summary>Incumplimiento de una regla de entrada o de negocio.</summary>
public sealed class ExcepcionDominio(string mensaje) : Exception(mensaje);
