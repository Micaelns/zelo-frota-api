using Domain.Interfaces.Repository;
using Infra.Repositories;
using Infra.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Extensions;

public static class ImplementsExtensions
{
    public static IServiceCollection ImplementsRepository(this IServiceCollection services)
    {
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
        services.AddScoped<ITravelRepository, TravelRepository>();
        services.AddScoped<IDestinationRepository, DestinationRepository>();
        return services;
    }
}
