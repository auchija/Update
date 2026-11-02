using System.ComponentModel.DataAnnotations;

namespace update.Application.DTOs;

public sealed class GuardarPublicacionFeedDto
{
    [Required(ErrorMessage = "Escribe algo antes de publicar.")]
    [StringLength(5000, ErrorMessage = "El limite es de 5000 caracteres.")]
    public string Contenido { get; set; } = string.Empty;
}

public sealed record PublicacionFeedDto(
    Guid Id, string Autor, string Contenido, DateTime CreadoEn,
    DateTime? ActualizadoEn, bool EsPropia);
