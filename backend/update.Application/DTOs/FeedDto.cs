using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using update.Application.Validation;

namespace update.Application.DTOs;

/// <summary>Lo que el usuario escribe al crear o editar una publicación.</summary>
public sealed class GuardarPublicacionDto
{
    public const int LimiteContenido = 5000;

    [JsonRequired]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Escribe algo antes de publicar.")]
    [MaxLength(LimiteContenido, ErrorMessage = "Usa máximo 5000 caracteres.")]
    public string Contenido { get; set; } = string.Empty;

    /// <summary>Imagen subida antes con POST /api/archivos/subir y proposito=publicacion; null la quita.</summary>
    [GuidNoVacio(ErrorMessage = "La imagen no es válida.")]
    public Guid? ImagenArchivoId { get; set; }
}

public sealed class AutorPublicacionDto
{
    public string NombreUsuario { get; set; } = string.Empty;
    public string NombreVisible { get; set; } = string.Empty;
    public string? FotoUrl { get; set; }
}

public sealed class RespuestaPublicacionFeedDto
{
    public Guid Id { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public Guid? ImagenArchivoId { get; set; }
    public string? ImagenUrl { get; set; }
    public AutorPublicacionDto Autor { get; set; } = new();
    /// <summary>true si la escribió quien hace la petición; el cliente solo muestra Editar y Eliminar en ese caso.</summary>
    public bool EsMia { get; set; }
    public DateTime CreadoEn { get; set; }
    public DateTime? EditadoEn { get; set; }
}
