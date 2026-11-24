using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Reporte : BaseEntity, IEntidadValidable
{
    public Guid UsuarioReportanteId { get; set; }
    public ObjetivoReporte ObjetivoTipo { get; set; }
    public Guid ObjetivoId { get; set; }
    public string MotivoCodigo { get; set; } = string.Empty;
    public string? Detalles { get; set; }
    public EstadoReporte Estado { get; set; } = EstadoReporte.Abierto;
    public Guid? ResueltoPorUsuarioId { get; set; }
    public string? NotaResolucion { get; set; }
    public DateTime? ResueltoEn { get; set; }
    [JsonIgnore]
    public MotivoReporte Motivo { get; set; } = null!;
    [JsonIgnore]
    public Usuario? ResueltoPorUsuario { get; set; }
    [JsonIgnore]
    public Usuario UsuarioReportante { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(UsuarioReportanteId != Guid.Empty, "UsuarioReportanteId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(ObjetivoTipo), "ObjetivoTipo contiene un valor no permitido.");
        Reglas.Exigir(ObjetivoId != Guid.Empty, "ObjetivoId es obligatorio.");
        Reglas.Texto(MotivoCodigo, nameof(MotivoCodigo), 64, true, false);
        Reglas.Texto(Detalles, nameof(Detalles), 4000, false, false);
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.Texto(NotaResolucion, nameof(NotaResolucion), 4000, false, false);
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ResueltoEn, nameof(ResueltoEn));
    }
}
