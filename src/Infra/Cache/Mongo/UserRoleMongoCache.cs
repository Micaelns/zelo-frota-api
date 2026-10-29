using Application.Contracts.Abstractions.Cache;
using Application.DTO.Authentic;
using Infra.Cache.Mongo.context;
using Infra.Cache.Mongo.documents;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infra.Cache.Mongo;

public class UserRoleMongoCache(AuthorizationMongoContext context, ILogger<UserRoleMongoCache> logger, IOptions<CacheSettings> optionsCache) : IUserRoleCache
{
    private readonly AuthorizationMongoContext _context = context;
    private readonly CacheSettings _cacheConfig = optionsCache.Value;
    private readonly ILogger<UserRoleMongoCache> _logger = logger;

    public async Task<IReadOnlyCollection<RoleSimpleDTO>> GetAsync(int userId, CancellationToken cancellationToken = default)
    {
        if (!IsActiveMongo())
        {
            return [];
        }

        var document = await _context.UserRoleCache
                    .Find(item => item.UserId == userId)
                    .FirstOrDefaultAsync(cancellationToken);

        if (document is null || document.ExpiresAt <= DateTime.UtcNow)
            return [];

        return document.Roles;
    }

    public async Task SetAsync(int userId, IReadOnlyCollection<RoleSimpleDTO> roles, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        if (!IsActiveMongo())
        {
            return;
        }

        var document = new UserRoleCacheDocument
        {
            Id = $"user-roles:{userId}",
            UserId = userId,
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.Add(expiration)
        };

        await _context.UserRoleCache.ReplaceOneAsync(
            x => x.UserId == userId,
            document,
            new ReplaceOptions
            {
                IsUpsert = true
            },
            cancellationToken);
    }

    private bool IsActiveMongo()
    {
        if (!_cacheConfig.Enabled)
        {
            _logger.LogWarning("Cache temporariamente inativo.");
            return false;
        }

        if (_cacheConfig.Provider != "Mongo")
        {
            _logger.LogError("Implementação incorreta do Cache com Mongo. Cache previsto seria {@Provider}", _cacheConfig.Provider);
            return false;
        }

        return true;
    }

}
