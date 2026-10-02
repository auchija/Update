using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de verificaciones.</summary>
public sealed class VerificacionRepository(ApplicationDbContext contexto) : GenericRepository<Verificacion>(contexto), IVerificacionRepository
{
    public async Task<IReadOnlyList<Verificacion>> ObtenerPorRevisadoPorUsuarioIdAsync(Guid? valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.RevisadoPorUsuarioId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<Verificacion>> ObtenerPorEstadoAsync(EstadoVerificacion valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
