using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla postulaciones.</summary>
public sealed class Postulacion : BaseEntity, IEntidadValidable
{
    /// <summary>Columna oportunidad_id (uuid); obligatoria.</summary>
    public Guid OportunidadId { get; set; }
    /// <summary>Columna perfil_postulante_id (uuid); obligatoria.</summary>
    public Guid PerfilPostulanteId { get; set; }
    /// <summary>Columna creado_por_usuario_id (uuid); obligatoria.</summary>
    public Guid CreadoPorUsuarioId { get; set; }
    /// <summary>Columna mensaje (text); obligatoria.</summary>
    public string Mensaje { get; set; } = string.Empty;
    /// <summary>Columna estado (estado_postulacion); obligatoria.</summary>
    public EstadoPostulacion Estado { get; set; } = EstadoPostulacion.Pendiente;
    /// <summary>Relación hacia oportunidades mediante oportunidad_id.</summary>
    [JsonIgnore]
    public Oportunidad Oportunidad { get; set; } = null!;
    /// <summary>Relación hacia perfiles mediante perfil_postulante_id.</summary>
    [JsonIgnore]
    public Perfil PerfilPostulante { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante creado_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(OportunidadId != Guid.Empty, "OportunidadId es obligatorio.");
        Reglas.Exigir(PerfilPostulanteId != Guid.Empty, "PerfilPostulanteId es obligatorio.");
        Reglas.Exigir(CreadoPorUsuarioId != Guid.Empty, "CreadoPorUsuarioId es obligatorio.");
        Reglas.Texto(Mensaje, nameof(Mensaje), 10000, true, false);
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
    }
}
