using Application.DTO.Authentic;

namespace Infra.Cache.Mongo.documents;

public class RoleCacheDocument
{
    public string Id { get; set; } = string.Empty;
    public IReadOnlyCollection<RoleDTO> Roles { get; set; } = [];
    public DateTime ExpiresAt { get; set; }
}
