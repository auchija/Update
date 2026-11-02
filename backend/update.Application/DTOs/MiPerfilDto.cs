using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using update.Application.Validation;

namespace update.Application.DTOs;

public sealed class ActualizarMiPerfilDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Elige un nombre de usuario.")]
    [RegularExpression("^[a-z0-9._]{3,30}$", ErrorMessage = "Usa de 3 a 30 caracteres: minúsculas, números, punto o guion bajo.")]
    public string NombreUsuario { get; set; } = string.Empty;

    [JsonRequired]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Escribe tu nombre.")]
    [StringLength(60, MinimumLength = 3, ErrorMessage = "Usa de 3 a 60 caracteres.")]
    public string NombreVisible { get; set; } = string.Empty;

    [MaxLength(255, ErrorMessage = "Usa máximo 255 caracteres.")]
    public string? Titular { get; set; }

    [MaxLength(2000, ErrorMessage = "Usa máximo 2000 caracteres.")]
    public string? Biografia { get; set; }

    [MaxLength(2048, ErrorMessage = "El enlace es demasiado largo.")]
    [Url(ErrorMessage = "Escribe un enlace que empiece por http:// o https://.")]
    public string? SitioWeb { get; set; }

    [GuidNoVacio]
    public Guid? FotoArchivoId { get; set; }
}

public sealed class RespuestaMiPerfilDto
{
    public Guid Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string NombreVisible { get; set; } = string.Empty;
    public string? Titular { get; set; }
    public string? Biografia { get; set; }
    public string? SitioWeb { get; set; }
    public Guid? FotoArchivoId { get; set; }
    public string? FotoUrl { get; set; }
    public int TotalSeguidores { get; set; }
    public int TotalSeguidos { get; set; }
    public int TotalPublicaciones { get; set; }
}

public sealed class RespuestaArchivoSubidoDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string TipoMime { get; set; } = string.Empty;
    public long TamanoBytes { get; set; }
}
