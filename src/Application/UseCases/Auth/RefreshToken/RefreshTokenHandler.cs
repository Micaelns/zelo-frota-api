using Application.Contracts.Abstractions;
using Application.DTO;
using Application.DTO.Authentic;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Auth.RefreshToken;

public class RefreshTokenHandler(IAuthentic authentic, ILogger<RefreshTokenHandler> logger) : IRequestHandler<RefreshTokenCommand, Result<UserAuthDTO>>
{
    private readonly IAuthentic _authentic = authentic;
    private readonly ILogger<RefreshTokenHandler> _logger = logger;

    public async Task<Result<UserAuthDTO>> Handle(
        RefreshTokenCommand query,
        CancellationToken cancellationToken)
    {
            
        var logon = await _authentic.RefreshAsync(query);

        if( logon.IsSuccess)
        {
            _logger.LogInformation("Logout realizado com sucesso.");
        }
        return logon;
    }
}
