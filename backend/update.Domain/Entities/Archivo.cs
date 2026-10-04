using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla archivos.</summary>
public sealed class Archivo : BaseEntity, IEntidadValidable, IEliminable
{
    /// <summary>Columna subido_por_usuario_id (uuid); obligatoria.</summary>
    public Guid SubidoPorUsuarioId { get; set; }
    /// <summary>Columna tipo (tipo_archivo); obligatoria.</summary>
    public TipoArchivo Tipo { get; set; }
    /// <summary>Columna proposito (proposito_archivo); obligatoria.</summary>
    public PropositoArchivo Proposito { get; set; }
    /// <summary>Columna proveedor_almacenamiento (text); obligatoria.</summary>
    public string ProveedorAlmacenamiento { get; set; } = "r2";
    /// <summary>Columna bucket (text); obligatoria.</summary>
    public string Bucket { get; set; } = string.Empty;
    /// <summary>Columna clave_objeto (text); obligatoria.</summary>
    public string ClaveObjeto { get; set; } = string.Empty;
    /// <summary>Columna nombre_original (text); opcional.</summary>
    public string? NombreOriginal { get; set; }
    /// <summary>Columna tipo_mime (text); obligatoria.</summary>
    public string TipoMime { get; set; } = string.Empty;
    /// <summary>Columna extension (text); obligatoria.</summary>
    public string Extension { get; set; } = string.Empty;
    /// <summary>Columna tamano_bytes (bigint); obligatoria.</summary>
    public long TamanoBytes { get; set; }
    /// <summary>Columna checksum_sha256 (char(64)); opcional.</summary>
    public string? ChecksumSha256 { get; set; }
    /// <summary>Columna ancho (integer); opcional.</summary>
    public int? Ancho { get; set; }
    /// <summary>Columna alto (integer); opcional.</summary>
    public int? Alto { get; set; }
    /// <summary>Columna duracion_segundos (numeric(8,2)); opcional.</summary>
    public decimal? DuracionSegundos { get; set; }
    /// <summary>Columna visibilidad (visibilidad_archivo); obligatoria.</summary>
    public VisibilidadArchivo Visibilidad { get; set; } = VisibilidadArchivo.Publico;
    /// <summary>Columna estado (estado_archivo); obligatoria.</summary>
    public EstadoArchivo Estado { get; set; } = EstadoArchivo.Pendiente;
    /// <summary>Columna eliminado_en (timestamptz); opcional.</summary>
    public DateTime? EliminadoEn { get; set; }
    /// <summary>Relación hacia usuarios mediante subido_por_usuario_id.</summary>
    [JsonIgnore]
    public Usuario SubidoPorUsuario { get; set; } = null!;

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
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

    /// <summary>Marca el registro como eliminado sin borrarlo físicamente.</summary>
    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
        Estado = EstadoArchivo.Eliminado;
    }

/// <summary>Completa el procesamiento de un archivo pendiente.</summary>
public void MarcarListo()
{
    Reglas.Exigir(Estado == EstadoArchivo.Pendiente, "Solo un archivo pendiente puede pasar a listo.");
    Estado = EstadoArchivo.Listo;
    ActualizadoEn = DateTime.UtcNow;
}
/// <summary>Registra un fallo durante el procesamiento.</summary>
public void MarcarFallido()
{
    Reglas.Exigir(Estado == EstadoArchivo.Pendiente, "Solo un archivo pendiente puede fallar.");
    Estado = EstadoArchivo.Fallido;
    ActualizadoEn = DateTime.UtcNow;
}
}
