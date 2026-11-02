using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using update.Domain.Enums;

namespace update.Application.DTOs;

/// <summary>Contrato de registro; coincide con el formulario del frontend.</summary>
public sealed class RegistroRequest
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [StringLength(60, MinimumLength = 3)]
    public string DisplayName { get; set; } = string.Empty;

    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [RegularExpression("^[a-z0-9._]{3,30}$", ErrorMessage = "Usa de 3 a 30 caracteres: minúsculas, números, punto o guion bajo.")]
    public string Handle { get; set; } = string.Empty;

    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [EmailAddress]
    [MaxLength(254)]
    public string Email { get; set; } = string.Empty;

    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [RegularExpression("^(?=.*[a-zA-Z])(?=.*[0-9]).{8,}$", ErrorMessage = "Usa al menos 8 caracteres, con una letra y un número.")]
    public string Password { get; set; } = string.Empty;

    [JsonRequired]
    [MinLength(1)]
    public string[] Personas { get; set; } = [];

    [JsonRequired]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Debes aceptar los términos.")]
    public bool TermsAccepted { get; set; }
}

/// <summary>Contrato de inicio de sesión; el identificador es el correo o el nombre de usuario.</summary>
public sealed class LoginRequest
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(254)]
    public string Identifier { get; set; } = string.Empty;

    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; } = string.Empty;
}

/// <summary>Usuario de la sesión, con los datos que muestra el frontend.</summary>
public sealed record UsuarioSesionDto(
    Guid Id,
    string Email,
    string Handle,
    string DisplayName,
    string? AvatarUrl,
    IReadOnlyList<string> Personas,
    RolPlataforma RolPlataforma);

/// <summary>Resultado interno para que el controlador establezca la cookie.</summary>
public sealed record AuthSessionResult(
    string AccessToken,
    UsuarioSesionDto User,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt);
