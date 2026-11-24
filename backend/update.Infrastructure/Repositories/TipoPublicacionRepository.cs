using Microsoft.EntityFrameworkCore;
using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

public sealed class TipoPublicacionRepository(ApplicationDbContext contexto) : GenericRepository<TipoPublicacion>(contexto), ITipoPublicacionRepository
{
    public async Task<IReadOnlyList<TipoPublicacion>> ObtenerPorCodigoAsync(string valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.Codigo == valor, pagina, tamanoPagina, cancellationToken);

    public async Task AsegurarAsync(string codigo, string nombre, CancellationToken cancellationToken = default)
        => await Contexto.Database.ExecuteSqlInterpolatedAsync($@"
            INSERT INTO tipos_publicacion (codigo, nombre)
            VALUES ({codigo}, {nombre})
            ON CONFLICT (codigo) DO NOTHING", cancellationToken);
}
