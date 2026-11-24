using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Participante : IEntidadValidable
{
    public Guid ConversacionId { get; set; }
    public Guid PerfilId { get; set; }
    public RolParticipante Rol { get; set; } = RolParticipante.Miembro;
    public Guid? UsuarioAsignadoId { get; set; }
    public DateTime UnidoEn { get; set; } = DateTime.UtcNow;
    public DateTime? SalioEn { get; set; }
    public Guid? UltimoMensajeLeidoId { get; set; }
    public DateTime? UltimaLecturaEn { get; set; }
    public DateTime? SilenciadoHasta { get; set; }
    public DateTime? ArchivadoEn { get; set; }
    [JsonIgnore]
    public Conversacion Conversacion { get; set; } = null!;
    [JsonIgnore]
    public Perfil Perfil { get; set; } = null!;
    [JsonIgnore]
    public Usuario? UsuarioAsignado { get; set; }
    [JsonIgnore]
    public Mensaje? UltimoMensajeLeido { get; set; }

    public void Validar()
    {
        Reglas.Exigir(ConversacionId != Guid.Empty, "ConversacionId es obligatorio.");
        Reglas.Exigir(PerfilId != Guid.Empty, "PerfilId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(Rol), "Rol contiene un valor no permitido.");
        Reglas.FechaUtc(UnidoEn, nameof(UnidoEn));
        Reglas.FechaUtc(SalioEn, nameof(SalioEn));
        Reglas.FechaUtc(UltimaLecturaEn, nameof(UltimaLecturaEn));
        Reglas.FechaUtc(SilenciadoHasta, nameof(SilenciadoHasta));
        Reglas.FechaUtc(ArchivadoEn, nameof(ArchivadoEn));
    }
}
