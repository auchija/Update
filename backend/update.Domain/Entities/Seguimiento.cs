using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla seguimientos.</summary>
public sealed class Seguimiento : IEntidadValidable
{
    /// <summary>Columna usuario_seguidor_id (uuid); obligatoria.</summary>
    public Guid UsuarioSeguidorId { get; set; }
    /// <summary>Columna perfil_seguido_id (uuid); obligatoria.</summary>
    public Guid PerfilSeguidoId { get; set; }
    /// <summary>Columna estado (estado_seguimiento); obligatoria.</summary>
    public EstadoSeguimiento Estado { get; set; } = EstadoSeguimiento.Activo;
    /// <summary>Columna creado_en (timestamptz); obligatoria.</summary>
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    /// <summary>Columna aceptado_en (timestamptz); opcional.</summary>
    public DateTime? AceptadoEn { get; set; }
    /// <summary>Relación hacia usuarios mediante usuario_seguidor_id.</summary>
    [JsonIgnore]
    public Usuario UsuarioSeguidor { get; set; } = null!;
    /// <summary>Relación hacia perfiles mediante perfil_seguido_id.</summary>
    [JsonIgnore]
    public Perfil PerfilSeguido { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(UsuarioSeguidorId != Guid.Empty, "UsuarioSeguidorId es obligatorio.");
        Reglas.Exigir(PerfilSeguidoId != Guid.Empty, "PerfilSeguidoId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(AceptadoEn, nameof(AceptadoEn));
    }

/// <summary>Acepta una solicitud de seguimiento pendiente.</summary>
public void Aceptar()
{
    Reglas.Exigir(Estado == EstadoSeguimiento.Pendiente, "El seguimiento no está pendiente.");
    Estado = EstadoSeguimiento.Activo;
    AceptadoEn = DateTime.UtcNow;
}
}
