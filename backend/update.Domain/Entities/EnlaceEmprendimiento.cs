using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla enlaces_emprendimiento.</summary>
public sealed class EnlaceEmprendimiento : BaseEntity, IEntidadValidable
{
    /// <summary>Columna emprendimiento_id (uuid); obligatoria.</summary>
    public Guid EmprendimientoId { get; set; }
    /// <summary>Columna plataforma (text); obligatoria.</summary>
    public string Plataforma { get; set; } = string.Empty;
    /// <summary>Columna url (text); obligatoria.</summary>
    public string Url { get; set; } = string.Empty;
    /// <summary>Columna orden (smallint); obligatoria.</summary>
    public short Orden { get; set; } = (short)0;
    /// <summary>Relación hacia emprendimientos mediante emprendimiento_id.</summary>
    [JsonIgnore]
    public Emprendimiento Emprendimiento { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(EmprendimientoId != Guid.Empty, "EmprendimientoId es obligatorio.");
        Reglas.Texto(Plataforma, nameof(Plataforma), 64, true, false);
        Reglas.Texto(Url, nameof(Url), 2048, true, false);
    }
}
