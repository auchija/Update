using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de tipos_oportunidad.</summary>
public interface ITipoOportunidadRepository : IRepository<TipoOportunidad>
{
    Task<IReadOnlyList<TipoOportunidad>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
