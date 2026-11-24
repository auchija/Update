using update.Domain.Entities;
using update.Domain.Enums;
namespace update.Domain.Interfaces;

/// <summary>Consultas específicas de publicaciones.</summary>
public interface IPublicacionRepository : IRepository<Publicacion>
{
    Task<IReadOnlyList<Publicacion>> ObtenerPorPerfilAutorIdAsync(Guid valor, int pagina = 1, int tamanoPagina = 20, CancellationToken cancellationToken = default);
    /// <summary>Publicaciones públicas, de la más nueva a la más antigua, con su autor y la foto del autor.</summary>
    /// <param name="usuarioId">Si se indica, solo las de ese usuario (incluidas las no públicas).</param>
    /// <param name="perfilAutorId">Si se indica, solo las públicas de ese perfil.</param>
    /// <param name="visorUsuarioId">Quien mira: ve las cuentas privadas propias y las que sigue.</param>
    Task<IReadOnlyList<Publicacion>> ObtenerRecientesAsync(int pagina, int tamanoPagina, CancellationToken cancellationToken = default, Guid? usuarioId = null, Guid? perfilAutorId = null, Guid? visorUsuarioId = null);
    /// <summary>Suma (o resta) a los contadores en una sola sentencia UPDATE, sin carreras entre peticiones.</summary>
    Task AjustarContadoresAsync(Guid publicacionId, int reacciones, int comentarios, CancellationToken cancellationToken = default);
}
