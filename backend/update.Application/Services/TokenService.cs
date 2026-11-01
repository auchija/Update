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
        var key = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Falta configurar Jwt:Key.");

        var issuer = configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Falta configurar Jwt:Issuer.");

        var audience = configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("Falta configurar Jwt:Audience.");

        var minutos = configuration.GetValue<int?>("Jwt:AccessTokenMinutes") ?? 15;
        var ahora = DateTime.UtcNow;

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.RolPlataforma.ToString())
        };

        var credenciales = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: ahora,
            expires: ahora.AddMinutes(minutos),
            signingCredentials: credenciales);

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
