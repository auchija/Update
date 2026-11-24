using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

public sealed class MensajeRepository(ApplicationDbContext contexto) : GenericRepository<Mensaje>(contexto), IMensajeRepository
{
    public async Task<IReadOnlyList<Mensaje>> ObtenerPorUsuarioRemitenteIdAsync(Guid? valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.UsuarioRemitenteId == valor, pagina, tamanoPagina, cancellationToken);
}
