using Microsoft.EntityFrameworkCore;
using update.Domain.Interfaces;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Infrastructure.Data;

namespace update.Infrastructure;

public sealed class UnitOfWork(ApplicationDbContext contexto) : IUnitOfWork
{
    public async Task<T> EjecutarEnTransaccionAsync<T>(
        Func<CancellationToken, Task<T>> operacion,
        CancellationToken cancellationToken)
    {
        var estrategia = contexto.Database.CreateExecutionStrategy();

        return await estrategia.ExecuteAsync(async () =>
        {
            await using var transaccion =
                await contexto.Database.BeginTransactionAsync(cancellationToken);

            var resultado = await operacion(cancellationToken);

            await transaccion.CommitAsync(cancellationToken);
            return resultado;
            // Si ocurre una excepción antes del Commit, DisposeAsync hace rollback.
        });
    }
}