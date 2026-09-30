using Application.Contracts.Abstractions;
using Application.DTO;
using Application.DTO.Authentic;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Auth.Logout;

public class LogoutHandler(IAuthentic authentic, ILogger<LogoutHandler> logger) : IRequestHandler<LogoutCommand, Result<AuthMessageDTO>>
{
    private readonly IAuthentic _authentic = authentic;
    private readonly ILogger<LogoutHandler> _logger = logger;

    public async Task<Result<AuthMessageDTO>> Handle(
        LogoutCommand command,
        CancellationToken cancellationToken)
    {
            
        var logon = await _authentic.LogoutAsync(command);

        if( logon.IsSuccess)
        {
            _logger.LogInformation("Logout realizado com sucesso.");
        }
        return logon;
    }
}
