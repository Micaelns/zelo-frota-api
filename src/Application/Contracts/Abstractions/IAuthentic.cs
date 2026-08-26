using Application.DTO;
using Application.DTO.Authentic;

namespace Application.Contracts.Abstractions;

public interface IAuthentic
{
    Task<Result<UserAuthDTO>> LogonAsync(LogonRequestDto logon);
}
