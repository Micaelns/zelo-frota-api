using Application.Contracts.Abstractions.Cache;
using Application.DTO.Authentic;
using Infra.Cache.Mongo.context;
using Infra.Cache.Mongo.documents;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Infra.Cache.Mongo;

public class RoleMongoCache(AuthorizationMongoContext context, ILogger<RoleMongoCache> logger, IOptions<CacheSettings> optionsCache) : IRoleCache
{
    private readonly AuthorizationMongoContext _context = context;
    private readonly CacheSettings _cacheConfig = optionsCache.Value;
    private readonly ILogger<RoleMongoCache> _logger = logger;

    public async Task<IReadOnlyCollection<RoleDTO>> GetAsync(CancellationToken cancellationToken = default)
    {
        if (!_cacheConfig.Enabled || _cacheConfig.Provider != "Mongo")
        {
            WriteLogger();
            return [];
        }

        var document = await _context.RoleCache
                    .Find(item => 1 == 1)
                    .FirstOrDefaultAsync(cancellationToken);

        if (document is null || document.ExpiresAt <= DateTime.UtcNow)
            return [];

        return document.Roles;

    }
    
    public async Task SetAsync(IReadOnlyCollection<RoleDTO> roles, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        if (!_cacheConfig.Enabled || _cacheConfig.Provider != "Mongo")
        {
            WriteLogger();
            return;
        }

        var document = new RoleCacheDocument
        {
            Id = $"roles:zf",
            Roles = roles,
            ExpiresAt = DateTime.UtcNow.Add(expiration)
        };

        await _context.RoleCache.ReplaceOneAsync(
            x => x.Id == $"roles:zf",
            document,
            new ReplaceOptions
            {
                IsUpsert = true
            },
            cancellationToken);
    }

    private void WriteLogger()
    {
        if (!_cacheConfig.Enabled)
        {
            _logger.LogWarning("Cache temporariamente inativo.");
            return;
        }
        if (_cacheConfig.Provider != "Mongo")
        {
            _logger.LogError("Implementação incorreta do Cache com Mongo. Cache previsto seria {@Provider}", _cacheConfig.Provider);
        }
    }

}
