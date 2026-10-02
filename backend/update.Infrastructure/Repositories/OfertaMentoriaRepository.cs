using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de ofertas_mentoria.</summary>
public sealed class OfertaMentoriaRepository(ApplicationDbContext contexto) : GenericRepository<OfertaMentoria>(contexto), IOfertaMentoriaRepository
{
    public async Task<IReadOnlyList<OfertaMentoria>> ObtenerPorUsuarioMentorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.UsuarioMentorId == valor, pagina, tamanoPagina, cancellationToken);
}
