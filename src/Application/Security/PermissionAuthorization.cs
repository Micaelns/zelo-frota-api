using Application.Contracts.Abstractions;

namespace Application.Security;

public class PermissionAuthorization(IRoles roleService) : IPermissionAuthorization
{
    public readonly IRoles _roleService = roleService;
    
    public async Task<bool> HasPermissionAsync(int userId, string permission)
    {
        var userRoles = await _roleService.RolesByUserAsync(userId);
        if (!userRoles.IsSuccess || userRoles.Value is null || !userRoles.Value.Any())
            return false;

        var allRoles = await _roleService.RolesAsync();
        if (!allRoles.IsSuccess || allRoles.Value is null || !allRoles.Value.Any())
            return false;

        var roles = allRoles.Value.Where(role =>
                                        userRoles.Value.Any(userRole => userRole.Name.Equals(role.Name, StringComparison.OrdinalIgnoreCase))
                                        &&
                                        role.Permissions.Any(RolePermission =>
                                                           RolePermission.Equals(permission, StringComparison.OrdinalIgnoreCase)
                                                            )
                                        );

        if (!roles.Any())
        {
            return false;
        }
        return true;
    }
}
