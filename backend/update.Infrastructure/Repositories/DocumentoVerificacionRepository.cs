using update.Domain.Entities;
using update.Domain.Enums;
using update.Domain.Interfaces;
using update.Infrastructure.Data;
namespace update.Infrastructure.Repositories;

/// <summary>Repositorio asíncrono de documentos_verificacion.</summary>
public sealed class DocumentoVerificacionRepository(ApplicationDbContext contexto) : GenericRepository<DocumentoVerificacion>(contexto), IDocumentoVerificacionRepository
{
    public async Task<IReadOnlyList<DocumentoVerificacion>> ObtenerPorVerificacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default)
        => await BuscarAsync(e => e.VerificacionId == valor, pagina, tamanoPagina, cancellationToken);
}
