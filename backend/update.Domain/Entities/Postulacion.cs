using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Postulacion : BaseEntity, IEntidadValidable
{
    public Guid OportunidadId { get; set; }
    public Guid PerfilPostulanteId { get; set; }
    public Guid CreadoPorUsuarioId { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public EstadoPostulacion Estado { get; set; } = EstadoPostulacion.Pendiente;
    [JsonIgnore]
    public Oportunidad Oportunidad { get; set; } = null!;
    [JsonIgnore]
    public Perfil PerfilPostulante { get; set; } = null!;
    [JsonIgnore]
    public Usuario CreadoPorUsuario { get; set; } = null!;

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
