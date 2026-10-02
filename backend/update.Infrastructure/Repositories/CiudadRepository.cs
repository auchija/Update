using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de ciudades.</summary>
public sealed class CiudadRepository(ApplicationDbContext contexto) : GenericRepository<Ciudad>(contexto), ICiudadRepository
{
    public async Task<IReadOnlyList<Ciudad>> ObtenerPorDepartamentoIdAsync(int valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.DepartamentoId == valor, pagina, tamanoPagina, cancellationToken);
}
