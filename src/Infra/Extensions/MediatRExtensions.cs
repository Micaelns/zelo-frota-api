using Application.UseCases.Vehicles.CreateVehicle;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Extensions;

public static class MediatRExtensions
{
    public static IServiceCollection RegisterMediatRUseCases(this IServiceCollection services, string? licenseKey)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateVehicleHandler).Assembly);
            cfg.LicenseKey = licenseKey;
        });
        return services;
    }
}
