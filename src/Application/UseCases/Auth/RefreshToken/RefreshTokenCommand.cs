using Application.DTO;
using Application.DTO.Authentic;
using MediatR;

namespace Application.UseCases.Auth.RefreshToken;

public class RefreshTokenCommand : IRequest<Result<UserAuthDTO>>
{
    public string RefreshToken { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
}
