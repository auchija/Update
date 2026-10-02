using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de paises.</summary>
public sealed class PaisRepository(ApplicationDbContext contexto) : GenericRepository<Pais>(contexto), IPaisRepository
{
    public async Task<IReadOnlyList<Pais>> ObtenerPorMonedaDefectoCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.MonedaDefectoCodigo == valor, pagina, tamanoPagina, cancellationToken);
}
