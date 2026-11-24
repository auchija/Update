using update.Domain.Common;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using update.Domain.Enums;
using update.Domain.Interfaces;
namespace update.Domain.Entities;

public sealed class Perfil : BaseEntity, IEntidadValidable, IEliminable
{
    public TipoPerfil Tipo { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string NombreVisible { get; set; } = string.Empty;
    public string? Titular { get; set; }
    public string? Biografia { get; set; }
    public Guid? FotoArchivoId { get; set; }
    public Guid? PortadaArchivoId { get; set; }
    public int? CiudadId { get; set; }
    public string? SitioWeb { get; set; }
    public bool EsPrivado { get; set; } = false;
    public EstadoCuenta Estado { get; set; } = EstadoCuenta.Activo;
    public int TotalSeguidores { get; set; } = 0;
    public int TotalSeguidos { get; set; } = 0;
    public int TotalPublicaciones { get; set; } = 0;
    public DateTime? EliminadoEn { get; set; }

    public Guid UsuarioId { get; set; }

    [JsonIgnore]
    public Usuario Usuario { get; set; } = null!;
    [JsonIgnore]
    public Ciudad? Ciudad { get; set; }
    [JsonIgnore]
    public Archivo? FotoArchivo { get; set; }
    [JsonIgnore]
    public Archivo? PortadaArchivo { get; set; }

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

    public void Eliminar()
    {
        EliminadoEn ??= DateTime.UtcNow;
        ActualizadoEn = DateTime.UtcNow;
        Estado = EstadoCuenta.Eliminado;
    }
}
