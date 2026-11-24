using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

public interface IOfertaMentoriaRepository : IRepository<OfertaMentoria>
{
    Task<IReadOnlyList<OfertaMentoria>> ObtenerPorUsuarioMentorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
