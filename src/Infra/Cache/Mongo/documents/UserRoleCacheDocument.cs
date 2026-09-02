using Application.DTO.Authentic;

namespace Infra.Cache.Mongo.documents;

public class UserRoleCacheDocument
{
    public string Id { get; set; } = string.Empty;
    public int UserId { get; set; }
    public IReadOnlyCollection<RoleSimpleDTO> Roles { get; set; } = [];
    public DateTime ExpiresAt { get; set; }
}
