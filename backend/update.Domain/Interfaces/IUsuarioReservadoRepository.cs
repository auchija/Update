using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de usuarios_reservados.</summary>
public interface IUsuarioReservadoRepository : IRepository<UsuarioReservado>
{
    Task<IReadOnlyList<UsuarioReservado>> ObtenerPorNombreUsuarioAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
