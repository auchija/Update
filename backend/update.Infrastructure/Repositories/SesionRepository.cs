using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

public sealed class SesionRepository(ApplicationDbContext contexto) : GenericRepository<Sesion>(contexto), ISesionRepository
{
    public async Task<IReadOnlyList<Sesion>> ObtenerPorUsuarioIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.UsuarioId == valor, pagina, tamanoPagina, cancellationToken);
}
