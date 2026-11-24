using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IVarianteArchivoRepository : IRepository<VarianteArchivo>
{
    Task<IReadOnlyList<VarianteArchivo>> ObtenerPorArchivoIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
