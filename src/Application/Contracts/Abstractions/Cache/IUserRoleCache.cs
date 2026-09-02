using Application.DTO.Authentic;

namespace Application.Contracts.Abstractions.Cache;

public interface IUserRoleCache
{
    Task<IReadOnlyCollection<RoleSimpleDTO>> GetAsync(int userId,
        CancellationToken cancellationToken = default);
    Task SetAsync(
        int userId,
        IReadOnlyCollection<RoleSimpleDTO> roles,
        TimeSpan expiration, 
        CancellationToken cancellationToken = default);
}
