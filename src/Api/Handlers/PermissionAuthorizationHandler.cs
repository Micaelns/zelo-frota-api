using Api.Providers;
using Application.Contracts.Abstractions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Api.Handlers;

public class PermissionAuthorizationHandler(IPermissionAuthorization permissionAuthorization)
    : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionAuthorization _permissionAuthorization = permissionAuthorization;

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdClaim?.Value))
            return;

        if (!int.TryParse(userIdClaim.Value, out var userId))
            return;

        var hasPermission = await _permissionAuthorization.HasPermissionAsync(userId, requirement.Permission);
        if (hasPermission) 
            context.Succeed(requirement);
        
    }
}
