using Application.Contracts.Messaging;
using Domain.Interfaces.Query;
using Domain.Interfaces.Repository;
using Infra.Data.Queries;
using Infra.Data.Repositories;
using Infra.Messaging.Kafka;
using Infra.Messaging.Kafka.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Extensions;

public static class ImplementsExtensions
{
    public static IServiceCollection ImplementsRepository(this IServiceCollection services)
    {
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>();
        services.AddScoped<IDestinationRepository, DestinationRepository>();
        services.AddScoped<ITravelQuery, TravelQuery>();

        return services;
    }

    public static IServiceCollection ImplementsServices(this IServiceCollection services)
    { 
        services.AddSingleton<IMessageProducer, KafkaProducer>();
        services.AddSingleton<IEventTopicMapper, EventTopicMapper>();

        return services;
    }
}
