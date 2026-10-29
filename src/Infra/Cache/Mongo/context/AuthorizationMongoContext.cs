using Infra.Cache.Mongo.documents;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infra.Cache.Mongo.context;

public class AuthorizationMongoContext
{
    private readonly ILogger<AuthorizationMongoContext> _logger;
    public IMongoCollection<RoleCacheDocument> RoleCache { get; }
    public IMongoCollection<UserRoleCacheDocument> UserRoleCache { get; }

    public AuthorizationMongoContext(
        ILogger<AuthorizationMongoContext> logger,
        IMongoClient client,
        IOptions<DbOptionMongo> options)
    {
        _logger = logger;

        var database = client.GetDatabase(options.Value.DatabaseName);
        RoleCache = database.GetCollection<RoleCacheDocument>("role_cache");
        UserRoleCache = database.GetCollection<UserRoleCacheDocument>("user_role_cache");
    }

    public async Task CreateIndexesAsync()
    {
        var roleIndex = new CreateIndexModel<RoleCacheDocument>(
            Builders<RoleCacheDocument>.IndexKeys
                .Ascending(x => x.ExpiresAt),
            new CreateIndexOptions
            {
                ExpireAfter = TimeSpan.Zero
            });


        var userRoleIndex = new CreateIndexModel<UserRoleCacheDocument>(
            Builders<UserRoleCacheDocument>.IndexKeys
                .Ascending(x => x.ExpiresAt),
            new CreateIndexOptions
            {
                ExpireAfter = TimeSpan.Zero
            });

        try
        {
            await RoleCache.Indexes.CreateOneAsync(roleIndex);
            await UserRoleCache.Indexes.CreateOneAsync(userRoleIndex);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao criar indexes: {@error}", ex.Message);
        }
    }
}
