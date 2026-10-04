using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de solicitudes_mentoria.</summary>
public sealed class SolicitudMentoriaRepository(ApplicationDbContext contexto) : GenericRepository<SolicitudMentoria>(contexto), ISolicitudMentoriaRepository
{
    public async Task<IReadOnlyList<SolicitudMentoria>> ObtenerPorOfertaIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.OfertaId == valor, pagina, tamanoPagina, cancellationToken);
    public async Task<IReadOnlyList<SolicitudMentoria>> ObtenerPorEstadoAsync(EstadoMentoria valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Estado == valor, pagina, tamanoPagina, cancellationToken);
}
