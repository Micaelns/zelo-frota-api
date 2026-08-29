using Application.DTO.Authentic;
using Refit;

namespace Infra.External.Authentic;

public interface IRoleApi
{
    [Get("/v1/Role/user/{userId}")]
    public Task<List<RoleDTO>> RolesByUserAsync(int userId, int softwareId);
}
