using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Archivo : BaseEntity, IEntidadValidable, IEliminable
{
    public Guid SubidoPorUsuarioId { get; set; }
    public TipoArchivo Tipo { get; set; }
    public PropositoArchivo Proposito { get; set; }
    public string ProveedorAlmacenamiento { get; set; } = "r2";
    public string Bucket { get; set; } = string.Empty;
    public string ClaveObjeto { get; set; } = string.Empty;
    public string? NombreOriginal { get; set; }
    public string TipoMime { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long TamanoBytes { get; set; }
    public string? ChecksumSha256 { get; set; }
    public int? Ancho { get; set; }
    public int? Alto { get; set; }
    public decimal? DuracionSegundos { get; set; }
    public VisibilidadArchivo Visibilidad { get; set; } = VisibilidadArchivo.Publico;
    public EstadoArchivo Estado { get; set; } = EstadoArchivo.Pendiente;
    public DateTime? EliminadoEn { get; set; }
    [JsonIgnore]
    public Usuario SubidoPorUsuario { get; set; } = null!;

    public void Validar()
    {
        Reglas.Exigir(SubidoPorUsuarioId != Guid.Empty, "SubidoPorUsuarioId es obligatorio.");
        Reglas.Exigir(Enum.IsDefined(Tipo), "Tipo contiene un valor no permitido.");
        Reglas.Exigir(Enum.IsDefined(Proposito), "Proposito contiene un valor no permitido.");
        Reglas.Texto(ProveedorAlmacenamiento, nameof(ProveedorAlmacenamiento), 255, true, false);
        Reglas.Texto(Bucket, nameof(Bucket), 255, true, false);
        Reglas.Texto(ClaveObjeto, nameof(ClaveObjeto), 1024, true, false);
        Reglas.Texto(NombreOriginal, nameof(NombreOriginal), 255, false, false);
        Reglas.Texto(TipoMime, nameof(TipoMime), 127, true, false);
        Reglas.Texto(Extension, nameof(Extension), 16, true, false);
        Reglas.Exigir(TamanoBytes >= 0, "TamanoBytes no puede ser negativo.");
        Reglas.Texto(ChecksumSha256, nameof(ChecksumSha256), 64, false, false);
        Reglas.Exigir(ChecksumSha256 is null || ChecksumSha256.Length == 64, "ChecksumSha256 debe tener 64 caracteres.");
        Reglas.Exigir(DuracionSegundos is null || DuracionSegundos >= 0, "DuracionSegundos no puede ser negativo.");
        Reglas.Exigir(Enum.IsDefined(Visibilidad), "Visibilidad contiene un valor no permitido.");
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
        Reglas.FechaUtc(EliminadoEn, nameof(EliminadoEn));
    }

    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
        Estado = EstadoArchivo.Eliminado;
    }

public void MarcarListo()
{
    Reglas.Exigir(Estado == EstadoArchivo.Pendiente, "Solo un archivo pendiente puede pasar a listo.");
    Estado = EstadoArchivo.Listo;
    ActualizadoEn = DateTime.UtcNow;
}
public void MarcarFallido()
{
    Reglas.Exigir(Estado == EstadoArchivo.Pendiente, "Solo un archivo pendiente puede fallar.");
    Estado = EstadoArchivo.Fallido;
    ActualizadoEn = DateTime.UtcNow;
}
}
