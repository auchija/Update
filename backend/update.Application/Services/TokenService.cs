using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using update.Domain.Entities;
using update.Domain.Interfaces;
using static Microsoft.IdentityModel.Tokens.SecurityAlgorithms;

namespace update.Application.Services;

public sealed class TokenService(IConfiguration configuration)
{
    public string CrearAccessToken(Usuario usuario)
    {
        var clave = configuration["Jwt:Clave"]
            ?? throw new InvalidOperationException("Falta configurar Jwt:Clave.");

        var emisor = configuration["Jwt:Emisor"] ?? "UpDate";
        var audiencia = configuration["Jwt:Audiencia"] ?? "UpDate.API";
        var minutos = configuration.GetValue<int?>("Jwt:AccessTokenMinutes") ?? 15;
        var ahora = DateTime.UtcNow;

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Correo),
            // Debe coincidir con RoleClaimType = "rol" en Program.cs
            new Claim("rol", usuario.RolPlataforma.ToString())
        };

        var credenciales = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave)),
            SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: emisor,
            audience: audiencia,
            claims: claims,
            notBefore: ahora,
            expires: ahora.AddMinutes(minutos),
            signingCredentials: credenciales);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
