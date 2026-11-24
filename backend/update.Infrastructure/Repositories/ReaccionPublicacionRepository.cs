using Microsoft.EntityFrameworkCore;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

public sealed class ReaccionPublicacionRepository(ApplicationDbContext contexto) : GenericRepository<ReaccionPublicacion>(contexto), IReaccionPublicacionRepository
{
    public async Task<IReadOnlyList<ReaccionPublicacion>> ObtenerPorPublicacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.PublicacionId == valor, pagina, tamanoPagina, cancellationToken);

    public async Task<IReadOnlyList<Guid>> ObtenerPublicacionesConReaccionAsync(Guid perfilId, IReadOnlyCollection<Guid> publicacionIds, CancellationToken cancellationToken = default)
        => publicacionIds.Count == 0 ? [] : await Conjunto.AsNoTracking()
            .Where(e => e.PerfilId == perfilId && publicacionIds.Contains(e.PublicacionId))
            .Select(e => e.PublicacionId)
            .ToListAsync(cancellationToken);

    public async Task AsegurarTipoAsync(string codigo, string nombre, string emoji, short orden, CancellationToken cancellationToken = default)
        => await Contexto.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO tipos_reaccion (codigo, nombre, emoji, orden)
            VALUES ({codigo}, {nombre}, {emoji}, {orden})
            ON CONFLICT (codigo) DO NOTHING", cancellationToken);

    public async Task<bool> AgregarSiNoExisteAsync(Guid publicacionId, Guid perfilId, string tipo, CancellationToken cancellationToken = default)
        => await Contexto.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO reacciones_publicacion (publicacion_id, perfil_id, tipo_reaccion_codigo)
            VALUES ({publicacionId}, {perfilId}, {tipo})
            ON CONFLICT (publicacion_id, perfil_id) DO NOTHING", cancellationToken) > 0;

    public async Task<bool> QuitarAsync(Guid publicacionId, Guid perfilId, CancellationToken cancellationToken = default)
        => await Conjunto.Where(e => e.PublicacionId == publicacionId && e.PerfilId == perfilId)
            .ExecuteDeleteAsync(cancellationToken) > 0;
}
