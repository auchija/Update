using update.Domain.Enums;

namespace update.Application.DTOs;

/// <summary>Un archivo del historial de GET /api/archivos/mios: sus metadatos y la URL para verlo o descargarlo.</summary>
public sealed class RespuestaArchivoHistorialDto
{
    public Guid Id { get; set; }
    public string? NombreOriginal { get; set; }
    public long TamanoBytes { get; set; }
    public string TipoMime { get; set; } = string.Empty;
    public DateTime CreadoEn { get; set; }
    public Guid SubidoPorUsuarioId { get; set; }
    public PropositoArchivo Proposito { get; set; }
    public string Url { get; set; } = string.Empty;
}
