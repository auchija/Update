using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de etiquetas.</summary>
public interface IEtiquetaRepository : IRepository<Etiqueta>
{
    Task<IReadOnlyList<Etiqueta>> ObtenerPorIdAsync(int valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
