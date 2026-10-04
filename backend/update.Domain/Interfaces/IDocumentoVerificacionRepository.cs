using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de documentos_verificacion.</summary>
public interface IDocumentoVerificacionRepository : IRepository<DocumentoVerificacion>
{
    Task<IReadOnlyList<DocumentoVerificacion>> ObtenerPorVerificacionIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
}
