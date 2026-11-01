namespace update.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<T> EjecutarEnTransaccionAsync<T>(
        Func<CancellationToken, Task<T>> operacion,
        CancellationToken cancellationToken);
}