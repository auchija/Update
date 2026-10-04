using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla bloqueos.</summary>
public sealed class Bloqueo : IEntidadValidable
{
    /// <summary>Columna usuario_bloqueador_id (uuid); obligatoria.</summary>
    public Guid UsuarioBloqueadorId { get; set; }
    /// <summary>Columna perfil_bloqueado_id (uuid); obligatoria.</summary>
    public Guid PerfilBloqueadoId { get; set; }
    /// <summary>Columna creado_en (timestamptz); obligatoria.</summary>
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    /// <summary>Relación hacia usuarios mediante usuario_bloqueador_id.</summary>
    [JsonIgnore]
    public Usuario UsuarioBloqueador { get; set; } = null!;
    /// <summary>Relación hacia perfiles mediante perfil_bloqueado_id.</summary>
    [JsonIgnore]
    public Perfil PerfilBloqueado { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(UsuarioBloqueadorId != Guid.Empty, "UsuarioBloqueadorId es obligatorio.");
        Reglas.Exigir(PerfilBloqueadoId != Guid.Empty, "PerfilBloqueadoId es obligatorio.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
    }
}
