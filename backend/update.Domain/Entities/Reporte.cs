using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla reportes.</summary>
public sealed class Reporte : BaseEntity, IEntidadValidable
{
    /// <summary>Columna usuario_reportante_id (uuid); obligatoria.</summary>
    public Guid UsuarioReportanteId { get; set; }
    /// <summary>Columna objetivo_tipo (objetivo_reporte); obligatoria.</summary>
    public ObjetivoReporte ObjetivoTipo { get; set; }
    /// <summary>Columna objetivo_id (uuid); obligatoria.</summary>
    public Guid ObjetivoId { get; set; }
    /// <summary>Columna motivo_codigo (text); obligatoria.</summary>
    public string MotivoCodigo { get; set; } = string.Empty;
    /// <summary>Columna detalles (text); opcional.</summary>
    public string? Detalles { get; set; }
    /// <summary>Columna estado (estado_reporte); obligatoria.</summary>
    public EstadoReporte Estado { get; set; } = EstadoReporte.Abierto;
    /// <summary>Columna resuelto_por_usuario_id (uuid); opcional.</summary>
    public Guid? ResueltoPorUsuarioId { get; set; }
    /// <summary>Columna nota_resolucion (text); opcional.</summary>
    public string? NotaResolucion { get; set; }
    /// <summary>Columna resuelto_en (timestamptz); opcional.</summary>
    public DateTime? ResueltoEn { get; set; }
    /// <summary>Relación hacia motivos_reporte mediante motivo_codigo.</summary>
    [JsonIgnore]
    public MotivoReporte Motivo { get; set; } = null!;
    /// <summary>Relación hacia usuarios mediante resuelto_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario? ResueltoPorUsuario { get; set; }
    /// <summary>Relación hacia usuarios mediante usuario_reportante_id.</summary>
    [JsonIgnore]
    public Usuario UsuarioReportante { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
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
