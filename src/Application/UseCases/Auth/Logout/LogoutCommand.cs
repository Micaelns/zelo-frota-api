using Application.DTO;
using Application.DTO.Authentic;
using MediatR;

namespace Application.UseCases.Auth.Logout;

public class LogoutCommand : IRequest<Result<AuthMessageDTO>>
{
    public string RefreshToken { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
}
