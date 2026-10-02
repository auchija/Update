using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de resenas_archivos.</summary>
public interface IResenaArchivoRepository : IRepository<ResenaArchivo>
{
    Task<IReadOnlyList<ResenaArchivo>> ObtenerPorResenaIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
