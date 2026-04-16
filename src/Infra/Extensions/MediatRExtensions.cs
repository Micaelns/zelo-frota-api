using Application.UseCases.Vehicles.CreateVehicle;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Extensions;

public static class MediatRExtensions
{
    public static IServiceCollection RegisterMediatRUseCases(this IServiceCollection Services)
    {
        Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateVehicleHandler).Assembly));
        return Services;
    }
}
