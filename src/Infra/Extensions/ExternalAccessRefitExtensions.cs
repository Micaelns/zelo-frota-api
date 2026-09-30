using Application.Contracts.Abstractions;
using Infra.External.Authentic;
using Infra.External.Authentic.Adapters;
using Infra.External.Authentic.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

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

        services.AddHttpContextAccessor();
        services.AddTransient<UserTokenHandler>();

        services.AddRefitGeneratedClient<IAuthenticApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUrl);
            })
            .AddHttpMessageHandler<UserTokenHandler>();

        services.AddRefitGeneratedClient<IRoleApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(baseUrl);
            })
            .AddHttpMessageHandler<UserTokenHandler>();

        services.AddScoped<IAuthentic, AuthenticApiAdapter>();
        services.AddSingleton<IRoles, RoleApiAdapter>();

        return services;
    }

}
