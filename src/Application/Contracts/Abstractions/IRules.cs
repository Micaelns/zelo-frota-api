using Application.DTO;
using Application.DTO.Authentic;

namespace Application.Contracts.Abstractions;

public interface IRules
{
    Task<Result<List<RoleDTO>>> RolesByUserAsync(int userId);
}
