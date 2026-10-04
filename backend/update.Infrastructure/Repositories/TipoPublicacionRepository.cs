using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de tipos_publicacion.</summary>
public sealed class TipoPublicacionRepository(ApplicationDbContext contexto) : GenericRepository<TipoPublicacion>(contexto), ITipoPublicacionRepository
{
    public async Task<IReadOnlyList<TipoPublicacion>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Codigo == valor, pagina, tamanoPagina, cancellationToken);
}
