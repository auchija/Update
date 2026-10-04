using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de tipos_oportunidad.</summary>
public sealed class TipoOportunidadRepository(ApplicationDbContext contexto) : GenericRepository<TipoOportunidad>(contexto), ITipoOportunidadRepository
{
    public async Task<IReadOnlyList<TipoOportunidad>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Codigo == valor, pagina, tamanoPagina, cancellationToken);
}
