using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de menciones.</summary>
public sealed class MencionRepository(ApplicationDbContext contexto) : GenericRepository<Mencion>(contexto), IMencionRepository
{
    public async Task<IReadOnlyList<Mencion>> ObtenerPorPublicacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.PublicacionId == valor, pagina, tamanoPagina, cancellationToken);
}
