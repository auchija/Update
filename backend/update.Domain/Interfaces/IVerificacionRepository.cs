using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de verificaciones.</summary>
public interface IVerificacionRepository : IRepository<Verificacion>
{
    Task<IReadOnlyList<Verificacion>> ObtenerPorRevisadoPorUsuarioIdAsync(Guid? valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Verificacion>> ObtenerPorEstadoAsync(EstadoVerificacion valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
