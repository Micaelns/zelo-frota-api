using Application.DTO.Authentic;

namespace Application.Contracts.Abstractions.Cache;

public interface IRoleCache
{
    Task<IReadOnlyCollection<RoleDTO>> GetAsync(CancellationToken cancellationToken = default);
    Task SetAsync(
        IReadOnlyCollection<RoleDTO> roles,
        TimeSpan expiration,
        CancellationToken cancellationToken = default);
}
