using Application.DTO;
using Application.DTO.Authentic;
using Application.UseCases.Auth.Logout;
using Application.UseCases.Auth.RefreshToken;

namespace Application.Contracts.Abstractions;

public interface IAuthentic
{
    Task<Result<UserAuthDTO>> LogonAsync(LogonRequestDto logon);
    Task<Result<AuthMessageDTO>> LogoutAsync(LogoutCommand logout);
    Task<Result<UserAuthDTO>> RefreshAsync(RefreshTokenCommand refreshTokenDTO);
}
