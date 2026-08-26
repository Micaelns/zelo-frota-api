using Application.Contracts.Abstractions;
using Infra.Adapters.Authentic;
using Infra.External.Authentic;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Infra.Extensions;

public static class ExternalAccessRefitExtensions
{
    public static IServiceCollection RegistryAuthenticRefit(this IServiceCollection services, string? apiPath)
    {
        services.AddRefitGeneratedClient<IAuthenticApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(apiPath ?? "https://localhost/");
            });

        services.AddScoped<IAuthentic, AuthenticApiAdapter>();
        return services;
    }

}
