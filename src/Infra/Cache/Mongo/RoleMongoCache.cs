using Application.Contracts.Abstractions.Cache;
using Application.DTO.Authentic;
using Infra.Cache.Mongo.context;
using Infra.Cache.Mongo.documents;
using MongoDB.Driver;

namespace Infra.Cache.Mongo;

public class RoleMongoCache(AuthorizationMongoContext context) : IRoleCache
{
    private readonly AuthorizationMongoContext _context = context;

    public async Task<IReadOnlyCollection<RoleDTO>> GetAsync(CancellationToken cancellationToken = default)
    {
        var document = await _context.RoleCache
                    .Find(item => 1 == 1)
                    .FirstOrDefaultAsync(cancellationToken);

        if (document is null || document.ExpiresAt <= DateTime.UtcNow)
            return [];

        return document.Roles;

    }

    public async Task SetAsync(IReadOnlyCollection<RoleDTO> roles, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
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
}
