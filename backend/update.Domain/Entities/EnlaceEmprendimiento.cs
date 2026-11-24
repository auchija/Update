using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class EnlaceEmprendimiento : BaseEntity, IEntidadValidable
{
    public Guid EmprendimientoId { get; set; }
    public string Plataforma { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public short Orden { get; set; } = (short)0;
    [JsonIgnore]
    public Emprendimiento Emprendimiento { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(EmprendimientoId != Guid.Empty, "EmprendimientoId es obligatorio.");
        Reglas.Texto(Plataforma, nameof(Plataforma), 64, true, false);
        Reglas.Texto(Url, nameof(Url), 2048, true, false);
    }
}
