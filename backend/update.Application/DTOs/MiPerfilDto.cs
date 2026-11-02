using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using update.Application.Validation;

namespace update.Application.DTOs;

public sealed class ActualizarMiPerfilDto
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [RegularExpression("^[a-z0-9._]{3,30}$", ErrorMessage = "Usa de 3 a 30 caracteres: minúsculas, números, punto o guion bajo.")]
    public string NombreUsuario { get; set; } = string.Empty;

    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [StringLength(60, MinimumLength = 3)]
    public string NombreVisible { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Titular { get; set; }

    [MaxLength(2000)]
    public string? Biografia { get; set; }

    [MaxLength(2048)]
    [Url]
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
