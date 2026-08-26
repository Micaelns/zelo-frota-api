using Application.DTO.Authentic;
using Refit;

namespace Infra.External.Authentic;

public interface IAuthenticApi
{
    [Post("/v1/Auth/logon")]
    Task<UserAuthDTO> LogonAsync(LogonRequestDto logon);
}
