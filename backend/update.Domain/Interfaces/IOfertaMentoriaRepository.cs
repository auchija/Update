using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de ofertas_mentoria.</summary>
public interface IOfertaMentoriaRepository : IRepository<OfertaMentoria>
{
    Task<IReadOnlyList<OfertaMentoria>> ObtenerPorUsuarioMentorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
