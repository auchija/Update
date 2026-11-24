using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Seguimiento : IEntidadValidable
{
    public Guid UsuarioSeguidorId { get; set; }
    public Guid PerfilSeguidoId { get; set; }
    public EstadoSeguimiento Estado { get; set; } = EstadoSeguimiento.Activo;
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? AceptadoEn { get; set; }
    [JsonIgnore]
    public Usuario UsuarioSeguidor { get; set; } = null!;
    [JsonIgnore]
    public Perfil PerfilSeguido { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(UsuarioSeguidorId != Guid.Empty, "UsuarioSeguidorId es obligatorio.");
        Reglas.Exigir(PerfilSeguidoId != Guid.Empty, "PerfilSeguidoId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(AceptadoEn, nameof(AceptadoEn));
    }

public void Aceptar()
{
    Reglas.Exigir(Estado == EstadoSeguimiento.Pendiente, "El seguimiento no está pendiente.");
    Estado = EstadoSeguimiento.Activo;
    AceptadoEn = DateTime.UtcNow;
}
}
