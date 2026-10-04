using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de paises.</summary>
public interface IPaisRepository : IRepository<Pais>
{
    Task<IReadOnlyList<Pais>> ObtenerPorMonedaDefectoCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
