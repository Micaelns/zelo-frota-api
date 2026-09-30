using Microsoft.AspNetCore.Http;

namespace Infra.External.Authentic.Handlers;

public class UserTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserTokenHandler(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var authorization =
            _httpContextAccessor.HttpContext?
                .Request.Headers.Authorization
                .ToString();

        if (!string.IsNullOrWhiteSpace(authorization))
        {
            request.Headers.TryAddWithoutValidation(
                "Authorization",
                authorization);
        }

        return await base.SendAsync(
            request,
            cancellationToken);
    }
}
