using update.Domain.Entities;
using update.Domain.Enums;

namespace update.Domain.Interfaces;

public interface IRefreshTokenRepository
{
    Task AgregarAsync(RefreshToken token, CancellationToken ct);
    Task<RefreshToken?> ObtenerPorHashAsync(string hash, CancellationToken ct);
    Task ActualizarAsync(RefreshToken token, CancellationToken ct);
}
