using Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Api.Providers;

public sealed class PermissionPolicyProvider(
    IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(
        string policyName)
    {
        var policy = await  GetPolicyDefaultAsync(policyName);

        if (policy is not null)
            return policy;

        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(
                new PermissionRequirement(policyName))
            .Build();
    }

    private async Task<AuthorizationPolicy?> GetPolicyDefaultAsync(
        string policyName)
    {
        return await base.GetPolicyAsync(policyName);
    }
}
