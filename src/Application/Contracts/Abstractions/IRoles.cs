using Application.DTO;
using Application.DTO.Authentic;

namespace Application.Contracts.Abstractions;

public interface IRoles
{
    Task<Result<List<RoleDTO>>> RolesAsync();
    Task<Result<List<RoleSimpleDTO>>> RolesByUserAsync(int userId);
}
