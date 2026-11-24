using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace update.Application.DTOs;

/// <summary>Lo que el usuario escribe al comentar una publicación.</summary>
public sealed class ComentarDto
{
    public const int LimiteContenido = 1000;

    [JsonRequired]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Escribe un comentario.")]
    [MaxLength(LimiteContenido, ErrorMessage = "Usa máximo 1000 caracteres.")]
    public string Contenido { get; set; } = string.Empty;
}

public sealed class RespuestaComentarioFeedDto
{
    public Guid Id { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public AutorPublicacionDto Autor { get; set; } = new();
    /// <summary>true si lo puede borrar quien hace la petición (es su comentario o su publicación).</summary>
    public bool PuedeEliminar { get; set; }
    public DateTime CreadoEn { get; set; }
}

public sealed class EstadoMeGustaDto
{
    public bool MeGusta { get; set; }
    public int TotalReacciones { get; set; }
}

/// <summary>Perfil que ve cualquier usuario con sesión.</summary>
public sealed class PerfilPublicoDto
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string NombreVisible { get; set; } = string.Empty;
    public string? Titular { get; set; }
    public string? Biografia { get; set; }
    public string? SitioWeb { get; set; }
    public string? FotoUrl { get; set; }
    public int TotalPublicaciones { get; set; }
    public int TotalSeguidores { get; set; }
    public int TotalSeguidos { get; set; }
    /// <summary>true si es el perfil de quien hace la petición.</summary>
    public bool EsMio { get; set; }
    /// <summary>true si quien hace la petición lo sigue (seguimiento aceptado).</summary>
    public bool LoSigo { get; set; }
    /// <summary>true si quien hace la petición envió una solicitud que aún no aceptan.</summary>
    public bool SolicitudPendiente { get; set; }
    public bool EsPrivado { get; set; }
    /// <summary>false en una cuenta privada que quien hace la petición no sigue.</summary>
    public bool PuedeVerContenido { get; set; }
}

public sealed class EstadoSeguimientoDto
{
    public bool LoSigo { get; set; }
    public bool SolicitudPendiente { get; set; }
    public int TotalSeguidores { get; set; }
}

/// <summary>Alguien que pidió seguir mi cuenta privada.</summary>
public sealed class SolicitudSeguimientoDto
{
    public PerfilPublicoDto Perfil { get; set; } = new();
    public DateTime CreadoEn { get; set; }
}
