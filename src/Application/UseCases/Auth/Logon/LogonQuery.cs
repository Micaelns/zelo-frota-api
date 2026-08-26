using Application.DTO;
using Application.DTO.Authentic;
using MediatR;

namespace Application.UseCases.Auth.Logon;

public class LogonQuery : IRequest<Result<UserAuthDTO>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
