using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

public sealed class TipoReaccionRepository(ApplicationDbContext contexto) : GenericRepository<TipoReaccion>(contexto), ITipoReaccionRepository
{
    public async Task<IReadOnlyList<TipoReaccion>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Codigo == valor, pagina, tamanoPagina, cancellationToken);
}
