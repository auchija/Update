using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Bloqueo : IEntidadValidable
{
    public Guid UsuarioBloqueadorId { get; set; }
    public Guid PerfilBloqueadoId { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public Usuario UsuarioBloqueador { get; set; } = null!;
    [JsonIgnore]
    public Perfil PerfilBloqueado { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(UsuarioBloqueadorId != Guid.Empty, "UsuarioBloqueadorId es obligatorio.");
        Reglas.Exigir(PerfilBloqueadoId != Guid.Empty, "PerfilBloqueadoId es obligatorio.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }
}
