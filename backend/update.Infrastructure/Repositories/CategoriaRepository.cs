using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de categorias.</summary>
public sealed class CategoriaRepository(ApplicationDbContext contexto) : GenericRepository<Categoria>(contexto), ICategoriaRepository
{
    public async Task<IReadOnlyList<Categoria>> ObtenerPorPadreIdAsync(int? valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.PadreId == valor, pagina, tamanoPagina, cancellationToken);
}
