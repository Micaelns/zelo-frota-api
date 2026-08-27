using Application.Contracts.Abstractions;
using Infra.Adapters.Authentic;
using Infra.External.Authentic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using static System.Collections.Specialized.BitVector32;

namespace Infra.Extensions;

public static class ExternalAccessRefitExtensions
{
    public static IServiceCollection RegistryAuthenticRefit(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("AuthenticSettings");
        services.Configure<AuthenticSettings>(section);

        var settings = section.Get<AuthenticSettings>() ?? new AuthenticSettings();
        var baseUrl = string.IsNullOrWhiteSpace(settings.URL)
            ? "https://localhost/"
            : settings.URL;

        services.AddRefitGeneratedClient<IAuthenticApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUrl);
            });

        services.AddScoped<IAuthentic, AuthenticApiAdapter>();
        return services;
    }

}
