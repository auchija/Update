using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

/// <summary>Representa un registro de la tabla perfiles.</summary>
public sealed class Perfil : BaseEntity, IEntidadValidable, IEliminable
{
    /// <summary>Columna tipo (tipo_perfil); obligatoria.</summary>
    public TipoPerfil Tipo { get; set; }
    /// <summary>Columna nombre_usuario (text); obligatoria.</summary>
    public string NombreUsuario { get; set; } = string.Empty;
    /// <summary>Columna nombre_visible (text); obligatoria.</summary>
    public string NombreVisible { get; set; } = string.Empty;
    /// <summary>Columna titular (text); opcional.</summary>
    public string? Titular { get; set; }
    /// <summary>Columna biografia (text); opcional.</summary>
    public string? Biografia { get; set; }
    /// <summary>Columna foto_archivo_id (uuid); opcional.</summary>
    public Guid? FotoArchivoId { get; set; }
    /// <summary>Columna portada_archivo_id (uuid); opcional.</summary>
    public Guid? PortadaArchivoId { get; set; }
    /// <summary>Columna ciudad_id (integer); opcional.</summary>
    public int? CiudadId { get; set; }
    /// <summary>Columna sitio_web (text); opcional.</summary>
    public string? SitioWeb { get; set; }
    /// <summary>Columna es_privado (boolean); obligatoria.</summary>
    public bool EsPrivado { get; set; } = false;
    /// <summary>Columna estado (estado_cuenta); obligatoria.</summary>
    public EstadoCuenta Estado { get; set; } = EstadoCuenta.Activo;
    /// <summary>Columna total_seguidores (integer); obligatoria.</summary>
    public int TotalSeguidores { get; set; } = 0;
    /// <summary>Columna total_seguidos (integer); obligatoria.</summary>
    public int TotalSeguidos { get; set; } = 0;
    /// <summary>Columna total_publicaciones (integer); obligatoria.</summary>
    public int TotalPublicaciones { get; set; } = 0;
    /// <summary>Columna eliminado_en (timestamptz); opcional.</summary>
    public DateTime? EliminadoEn { get; set; }
    /// <summary>Relación hacia ciudades mediante ciudad_id.</summary>
    [JsonIgnore]
    public Ciudad? Ciudad { get; set; }
    /// <summary>Relación hacia archivos mediante foto_archivo_id.</summary>
    [JsonIgnore]
    public Archivo? FotoArchivo { get; set; }
    /// <summary>Relación hacia archivos mediante portada_archivo_id.</summary>
    [JsonIgnore]
    public Archivo? PortadaArchivo { get; set; }

    /// <summary>Comprueba las reglas de entrada antes de guardar.</summary>
    public void Validar()
    {
        Reglas.Exigir(Enum.IsDefined(Tipo), "Tipo contiene un valor no permitido.");
        Reglas.Texto(NombreUsuario, nameof(NombreUsuario), 50, true, false);
        Reglas.Texto(NombreVisible, nameof(NombreVisible), 255, true, false);
        Reglas.Texto(Titular, nameof(Titular), 255, false, false);
        Reglas.Texto(Biografia, nameof(Biografia), 2000, false, false);
        Reglas.Texto(SitioWeb, nameof(SitioWeb), 2048, false, false);
        Reglas.Exigir(Enum.IsDefined(Estado), "Estado contiene un valor no permitido.");
        Reglas.Exigir(TotalSeguidores >= 0, "TotalSeguidores no puede ser negativo.");
        Reglas.Exigir(TotalSeguidos >= 0, "TotalSeguidos no puede ser negativo.");
        Reglas.Exigir(TotalPublicaciones >= 0, "TotalPublicaciones no puede ser negativo.");
        Reglas.FechaUtc(CreadoEn, nameof(CreadoEn));
        Reglas.FechaUtc(ActualizadoEn, nameof(ActualizadoEn));
        Reglas.FechaUtc(EliminadoEn, nameof(EliminadoEn));
    }

    /// <summary>Marca el registro como eliminado sin borrarlo físicamente.</summary>
    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
        Estado = EstadoCuenta.Eliminado;
    }
}
