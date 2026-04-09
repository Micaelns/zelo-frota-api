using Domain.Interfaces.Repository;
using Infra.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Extensions;

public static class ImplementsExtensions
{
    public static IServiceCollection ImplementsRepository(this IServiceCollection services)
    {
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
        return services;
    }
}
