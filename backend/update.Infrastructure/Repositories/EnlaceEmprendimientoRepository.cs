using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de enlaces_emprendimiento.</summary>
public sealed class EnlaceEmprendimientoRepository(ApplicationDbContext contexto) : GenericRepository<EnlaceEmprendimiento>(contexto), IEnlaceEmprendimientoRepository
{
    public async Task<IReadOnlyList<EnlaceEmprendimiento>> ObtenerPorEmprendimientoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.EmprendimientoId == valor, pagina, tamanoPagina, cancellationToken);
}
