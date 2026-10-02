using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de monedas.</summary>
public sealed class MonedaRepository(ApplicationDbContext contexto) : GenericRepository<Moneda>(contexto), IMonedaRepository
{
    public async Task<IReadOnlyList<Moneda>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Codigo == valor, pagina, tamanoPagina, cancellationToken);
}
