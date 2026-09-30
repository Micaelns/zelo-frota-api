using Application.DTO.Authentic;
using Application.UseCases.Auth.Logout;
using Application.UseCases.Auth.RefreshToken;
using Refit;

namespace Infra.External.Authentic;

public interface IAuthenticApi
{
    [Post("/v1/Auth/logon")]
    Task<UserAuthDTO> LogonAsync(LogonRequestDto logon);

    [Post("/v1/Auth/logout")]
    Task<AuthMessageDTO> LogoutAsync(LogoutCommand logon);

    [Post("/v1/Auth/refresh")]
    Task<UserAuthDTO> RefreshAsync(RefreshTokenCommand logon);
}