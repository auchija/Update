using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de tipos_persona.</summary>
public interface ITipoPersonaRepository : IRepository<TipoPersona>
{
    Task<IReadOnlyList<TipoPersona>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
