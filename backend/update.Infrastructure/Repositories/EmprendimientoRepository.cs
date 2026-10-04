using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de emprendimientos.</summary>
public sealed class EmprendimientoRepository(ApplicationDbContext contexto) : GenericRepository<Emprendimiento>(contexto), IEmprendimientoRepository
{
    public async Task<IReadOnlyList<Emprendimiento>> ObtenerPorCategoriaIdAsync(int valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.CategoriaId == valor, pagina, tamanoPagina, cancellationToken);
}
