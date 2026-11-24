using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Verificacion : BaseEntity, IEntidadValidable
{
    public Guid EmprendimientoId { get; set; }
    public EstadoVerificacion Estado { get; set; } = EstadoVerificacion.Pendiente;
    public string Metodo { get; set; } = "documentos";
    public Guid EnviadoPorUsuarioId { get; set; }
    public Guid? RevisadoPorUsuarioId { get; set; }
    public string? NotasRevisor { get; set; }
    public DateTime EnviadoEn { get; set; } = DateTime.UtcNow;
    public DateTime? RevisadoEn { get; set; }
    [JsonIgnore]
    public Usuario? RevisadoPorUsuario { get; set; }
    [JsonIgnore]
    public Emprendimiento Emprendimiento { get; set; } = null!;
    [JsonIgnore]
    public Usuario EnviadoPorUsuario { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(EmprendimientoId != Guid.Empty, "EmprendimientoId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.Texto(Metodo, nameof(Metodo), 64, true, false);
        Reglas.Exigir(EnviadoPorUsuarioId != Guid.Empty, "EnviadoPorUsuarioId es obligatorio.");
        Reglas.Texto(NotasRevisor, nameof(NotasRevisor), 4000, false, false);
        Reglas.FechaUtc(EnviadoEn, nameof(EnviadoEn));
        Reglas.FechaUtc(RevisadoEn, nameof(RevisadoEn));
    }
}
