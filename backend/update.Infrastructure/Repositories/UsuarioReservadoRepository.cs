using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de usuarios_reservados.</summary>
public sealed class UsuarioReservadoRepository(ApplicationDbContext contexto) : GenericRepository<UsuarioReservado>(contexto), IUsuarioReservadoRepository
{
    public async Task<IReadOnlyList<UsuarioReservado>> ObtenerPorNombreUsuarioAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.NombreUsuario == valor, pagina, tamanoPagina, cancellationToken);
}
