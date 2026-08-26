using Application.Contracts.Abstractions;
using Application.DTO;
using Application.DTO.Authentic;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Auth.Logon;

public class LogonHandler(IAuthentic authentic, ILogger<LogonHandler> logger) : IRequestHandler<LogonQuery, Result<UserAuthDTO>>
{
    private readonly IAuthentic _authentic = authentic;
    private readonly ILogger<LogonHandler> _logger = logger;

    public async Task<Result<UserAuthDTO>> Handle(
        LogonQuery query,
        CancellationToken cancellationToken)
    {
            
        var logon = await _authentic.LogonAsync(new() { Email = query.Email, Password = query.Password});
        

        if( !logon.IsSuccess)
        {
            _logger.LogWarning(logon.Error);
        }
        else
        {
            _logger.LogInformation("Logon realizado com sucesso.");
        }
        return logon;
    }
}
