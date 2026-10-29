using Infra.External.Authentic;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IntegrationsTests;

public static class TestTokenGenerator
{
    public static string Execute(IConfiguration configuration, int userId)
    {
        var settings = configuration.GetSection("AuthenticSettings").Get<AuthenticSettings>()
                                     ?? throw new InvalidOperationException("AuthenticSettings não configurado.");

        var secret = settings.JwtSecret?? throw new InvalidOperationException("JwtSecret não configurado.");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                userId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: settings.JwtIssuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}
