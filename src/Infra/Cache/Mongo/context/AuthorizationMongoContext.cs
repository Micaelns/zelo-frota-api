using Infra.Cache.Mongo.documents;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infra.Cache.Mongo.context;

public class AuthorizationMongoContext
{
    public IMongoCollection<RoleCacheDocument> RoleCache { get; }
    public IMongoCollection<UserRoleCacheDocument> UserRoleCache { get; }

    public AuthorizationMongoContext(
        IMongoClient client,
        IOptions<DbOptionMongo> options)
    {
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

        await RoleCache.Indexes.CreateOneAsync(roleIndex);

        var userRoleIndex = new CreateIndexModel<UserRoleCacheDocument>(
            Builders<UserRoleCacheDocument>.IndexKeys
                .Ascending(x => x.ExpiresAt),
            new CreateIndexOptions
            {
                ExpireAfter = TimeSpan.Zero
            });

        await UserRoleCache.Indexes.CreateOneAsync(userRoleIndex);
    }
}
