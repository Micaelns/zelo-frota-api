using Application.Contracts.Abstractions.Cache;
using Infra.Cache.Mongo;
using Infra.Cache.Mongo.context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Infra.Extensions;

public static class RegisterCacheExtension
{
    public static IServiceCollection RegisterMongoCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterMongo(configuration);
        services.AddSingleton<AuthorizationMongoContext>();
        services.AddSingleton<IRoleCache, RoleMongoCache>();
        services.AddSingleton<IUserRoleCache, UserRoleMongoCache>();

        return services;
    }

    private static IServiceCollection RegisterMongo(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoSection = configuration.GetSection("Cache:Mongo");
        var settings = mongoSection.Get<DbOptionMongo>() ?? new DbOptionMongo();

        services.Configure<DbOptionMongo>(mongoSection);
        services.AddSingleton<IMongoClient>(_ => new MongoClient(settings.ConnectionString));

        return services;
    }
}
