using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de postulaciones.</summary>
public sealed class PostulacionRepository(ApplicationDbContext contexto) : GenericRepository<Postulacion>(contexto), IPostulacionRepository
{
    public async Task<IReadOnlyList<Postulacion>> ObtenerPorOportunidadIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.OportunidadId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<Postulacion>> ObtenerPorEstadoAsync(EstadoPostulacion valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
