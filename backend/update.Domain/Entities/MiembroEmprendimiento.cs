using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla miembros_emprendimiento.</summary>
public sealed class MiembroEmprendimiento : IEntidadValidable
{
    /// <summary>Columna emprendimiento_id (uuid); obligatoria.</summary>
    public Guid EmprendimientoId { get; set; }
    /// <summary>Columna usuario_id (uuid); obligatoria.</summary>
    public Guid UsuarioId { get; set; }
    /// <summary>Columna rol (rol_emprendimiento); obligatoria.</summary>
    public RolEmprendimiento Rol { get; set; } = RolEmprendimiento.Miembro;
    /// <summary>Columna titulo (text); opcional.</summary>
    public string? Titulo { get; set; }
    /// <summary>Columna estado (estado_miembro); obligatoria.</summary>
    public EstadoMiembro Estado { get; set; } = EstadoMiembro.Invitado;
    /// <summary>Columna invitado_por_usuario_id (uuid); opcional.</summary>
    public Guid? InvitadoPorUsuarioId { get; set; }
    /// <summary>Columna creado_en (timestamptz); obligatoria.</summary>
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    /// <summary>Columna unido_en (timestamptz); opcional.</summary>
    public DateTime? UnidoEn { get; set; }
    /// <summary>Columna actualizado_en (timestamptz); obligatoria.</summary>
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
    /// <summary>Relación hacia emprendimientos mediante emprendimiento_id.</summary>
    [JsonIgnore]
    public Emprendimiento Emprendimiento { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante usuario_id.</summary>
    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante invitado_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario? InvitadoPorUsuario { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(EmprendimientoId != Guid.Empty, "EmprendimientoId es obligatorio.");
        Reglas.Exigir(UsuarioId != Guid.Empty, "UsuarioId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(Rol), "Rol contiene un valor no permitido.");
        Reglas.Texto(Titulo, nameof(Titulo), 255, false, false);
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(UnidoEn, nameof(UnidoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
    }

/// <summary>Acepta una invitación pendiente al emprendimiento.</summary>
public void AceptarInvitacion()
{
    Reglas.Exigir(Estado == EstadoMiembro.Invitado, "No hay una invitación pendiente.");
    Estado = EstadoMiembro.Activo;
    UnidoEn = DateTime.UtcNow;
    ActualizadoEn = DateTime.UtcNow;
}
}
