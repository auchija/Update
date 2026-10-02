using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de ciudades.</summary>
public interface ICiudadRepository : IRepository<Ciudad>
{
    Task<IReadOnlyList<Ciudad>> ObtenerPorDepartamentoIdAsync(int valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
