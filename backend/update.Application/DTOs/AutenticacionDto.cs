using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using update.Application.Validation;
using update.Domain.Entities;

namespace update.Application.DTOs;

/// <summary>Contrato de registro; solo incluye los datos necesarios para crear la cuenta.</summary>
public sealed class RegistroRequest
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(254)]
    [EmailAddress]
    public string Correo { get; set; } = string.Empty;

    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    public string Contrasena { get; set; } = string.Empty;

    [JsonRequired]
    [FechaUtc]
    public DateTime TerminosAceptadosEn { get; set; }
}

/// <summary>Contrato de inicio de sesión.</summary>
public sealed class LoginRequest
{
    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    [MaxLength(254)]
    [EmailAddress]
    public string Correo { get; set; } = string.Empty;

    [JsonRequired]
    [Required(AllowEmptyStrings = false)]
    public string Contrasena { get; set; } = string.Empty;
}

/// <summary>Contrato de respuesta pública de autenticación.</summary>
public sealed class AuthResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public RespuestaUsuarioDto User { get; init; } = default!;

    public AuthResponse(string accessToken, RespuestaUsuarioDto user)
    {
        AccessToken = accessToken;
        User = user;
    }
}

/// <summary>Resultado interno para que el controlador establezca la cookie.</summary>
public sealed record AuthSessionResult(
    string AccessToken,
    RespuestaUsuarioDto User,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt);
