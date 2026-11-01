namespace update.Domain.Exceptions;

public sealed class CuentaBloqueadaException(DateTime bloqueadoHasta) : Exception("Cuenta bloqueada temporalmente.")
{
    public DateTime BloqueadoHasta { get; } = bloqueadoHasta;
}