using Microsoft.EntityFrameworkCore;
using update.Domain.Entities;
using update.Domain.Interfaces;
using update.Infrastructure.Data;

namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de tokens de actualización.</summary>
public sealed class RefreshTokenRepository(ApplicationDbContext contexto)
    : IRefreshTokenRepository
{
    public async Task AgregarAsync(RefreshToken token, CancellationToken ct)
    {
        await contexto.RefreshTokens.AddAsync(token, ct);
        await contexto.SaveChangesAsync(ct);
    }

    public Task<RefreshToken?> ObtenerPorHashAsync(string hash, CancellationToken ct)
        => contexto.RefreshTokens
            .FirstOrDefaultAsync(t => t.Hash == hash, ct);

    public async Task ActualizarAsync(RefreshToken token, CancellationToken ct)
    {
        contexto.RefreshTokens.Update(token);
        await contexto.SaveChangesAsync(ct);
    }
}
