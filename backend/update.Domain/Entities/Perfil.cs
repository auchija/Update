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
    /// <summary>Columna tipo (tipo_perfil); obligatoria. Valores permitidos: Persona, Emprendimiento.</summary>
    public TipoPerfil Tipo { get; set; }
    /// <summary>Columna nombre_usuario (text); obligatoria. Máximo 50 caracteres.</summary>
    public string NombreUsuario { get; set; } = string.Empty;
    /// <summary>Columna nombre_visible (text); obligatoria. Máximo 255 caracteres.</summary>
    public string NombreVisible { get; set; } = string.Empty;
    /// <summary>Columna titular (text); opcional. Máximo 255 caracteres. Información sobre el titular del perfil.</summary>
    public string? Titular { get; set; }
    /// <summary>Columna biografia (text); opcional. Máximo 2000 caracteres. Descripción breve del usuario.</summary>
    public string? Biografia { get; set; }
    /// <summary>Columna foto_archivo_id (uuid); opcional. Referencia a la imagen de perfil en tabla archivos.</summary>
    public Guid? FotoArchivoId { get; set; }
    /// <summary>Columna portada_archivo_id (uuid); opcional. Referencia a la imagen de portada en tabla archivos.</summary>
    public Guid? PortadaArchivoId { get; set; }
    /// <summary>Columna ciudad_id (integer); opcional. Referencia a la ciudad del usuario.</summary>
    public int? CiudadId { get; set; }
    /// <summary>Columna sitio_web (text); opcional. URL del sitio web personal o empresarial. Máximo 2048 caracteres.</summary>
    public string? SitioWeb { get; set; }
    /// <summary>Columna es_privado (boolean); obligatoria. Indica si el perfil está privado por defecto.</summary>
    public bool EsPrivado { get; set; } = false;
    /// <summary>Columna estado (estado_cuenta); obligatoria. Valores permitidos: Activo, Suspendido, Eliminado.</summary>
    public EstadoCuenta Estado { get; set; } = EstadoCuenta.Activo;
    /// <summary>Columna total_seguidores (integer); obligatoria. Número de seguidores. No puede ser negativo.</summary>
    public int TotalSeguidores { get; set; } = 0;
    /// <summary>Columna total_seguidos (integer); obligatoria. Número de usuarios que siguen este perfil. No puede ser negativo.</summary>
    public int TotalSeguidos { get; set; } = 0;
    /// <summary>Columna total_publicaciones (integer); obligatoria. Número de publicaciones del usuario. No puede ser negativo.</summary>
    public int TotalPublicaciones { get; set; } = 0;
    /// <summary>Columna eliminado_en (timestamptz); opcional. Fecha y hora de eliminación soft.</summary>
    public DateTime? EliminadoEn { get; set; }

    /// <summary> Identificador del usuario propietario del perfil. Cada perfil pertenece obligatoriamente a un usuario.</summary>
    public Guid UsuarioId { get; set; }

    /// <summary>Usuario propietario del perfil.</summary>
    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;
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
