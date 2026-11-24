using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

public sealed class TipoPersonaRepository(ApplicationDbContext contexto) : GenericRepository<TipoPersona>(contexto), ITipoPersonaRepository
{
    public async Task<IReadOnlyList<TipoPersona>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Codigo == valor, pagina, tamanoPagina, cancellationToken);
}
