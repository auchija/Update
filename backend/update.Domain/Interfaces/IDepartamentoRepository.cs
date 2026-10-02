using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de departamentos.</summary>
public interface IDepartamentoRepository : IRepository<Departamento>
{
    Task<IReadOnlyList<Departamento>> ObtenerPorPaisCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
