using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class MiembroEmprendimiento : IEntidadValidable
{
    public Guid EmprendimientoId { get; set; }
    public Guid UsuarioId { get; set; }
    public RolEmprendimiento Rol { get; set; } = RolEmprendimiento.Miembro;
    public string? Titulo { get; set; }
    public EstadoMiembro Estado { get; set; } = EstadoMiembro.Invitado;
    public Guid? InvitadoPorUsuarioId { get; set; }
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? UnidoEn { get; set; }
    public DateTime ActualizadoEn { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public Emprendimiento Emprendimiento { get; set; } = null!;
    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;
    [JsonIgnore]
    public Usuario? InvitadoPorUsuario { get; set; }

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

public void AceptarInvitacion()
{
    Reglas.Exigir(Estado == EstadoMiembro.Invitado, "No hay una invitación pendiente.");
    Estado = EstadoMiembro.Activo;
    UnidoEn = DateTime.UtcNow;
    ActualizadoEn = DateTime.UtcNow;
}
}
