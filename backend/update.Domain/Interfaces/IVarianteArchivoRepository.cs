using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de variantes_archivo.</summary>
public interface IVarianteArchivoRepository : IRepository<VarianteArchivo>
{
    Task<IReadOnlyList<VarianteArchivo>> ObtenerPorArchivoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
