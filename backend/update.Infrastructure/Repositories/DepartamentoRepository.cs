using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de departamentos.</summary>
public sealed class DepartamentoRepository(ApplicationDbContext contexto) : GenericRepository<Departamento>(contexto), IDepartamentoRepository
{
    public async Task<IReadOnlyList<Departamento>> ObtenerPorPaisCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.PaisCodigo == valor, pagina, tamanoPagina, cancellationToken);
}
