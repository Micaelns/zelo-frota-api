using Application.DTO;
using Application.DTO.Authentic;
using Refit;

namespace Infra.External.Authentic;

public interface IRoleApi
{
    [Get("/v1/Role/software/{softwareId}")]
    public Task<List<RoleDTO>> RolesAsync(int softwareId);

    [Get("/v1/Role/user/{userId}")]
    public Task<List<RoleSimpleDTO>> RolesByUserAsync(int userId, int softwareId);
}
