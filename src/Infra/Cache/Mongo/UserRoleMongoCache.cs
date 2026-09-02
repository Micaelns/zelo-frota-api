using Application.Contracts.Abstractions.Cache;
using Application.DTO.Authentic;
using Infra.Cache.Mongo.context;
using Infra.Cache.Mongo.documents;
using MongoDB.Driver;

namespace Infra.Cache.Mongo;

public class UserRoleMongoCache(AuthorizationMongoContext context) : IUserRoleCache
{
    private readonly AuthorizationMongoContext _context = context;

    public async Task<IReadOnlyCollection<RoleSimpleDTO>> GetAsync(int userId, CancellationToken cancellationToken = default)
    {
        var document = await _context.UserRoleCache
                    .Find(item => item.UserId == userId)
                    .FirstOrDefaultAsync(cancellationToken);

        if (document is null || document.ExpiresAt <= DateTime.UtcNow)
            return [];

        return document.Roles;
    }

    public async Task SetAsync(int userId, IReadOnlyCollection<RoleSimpleDTO> roles, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
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
}
