using Infra.External.Authentic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infra.Extensions;
public static class AuthorizationExtensions
{
    public static IServiceCollection AddInfrastructureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("AuthenticSettings").Get<AuthenticSettings>() ?? new AuthenticSettings();

        var key = settings.JwtSecret ?? throw new Exception("JWT Key not configured");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = settings.JwtIssuer,
                    ValidAudience = settings.JwtAudience,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(key))
                };
            });

        return services;
    }

}
