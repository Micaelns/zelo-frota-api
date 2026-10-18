using Api.Helper;
using Application.UseCases.Auth.Logon;
using Application.UseCases.Auth.Logout;
using Application.UseCases.Auth.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IMediator mediator, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost]
    [AllowAnonymous]
    [Route("logon")]
    public async Task<IActionResult> Logon([FromBody] LogonQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema no logon. {@query}; {@errorType} : {@error}", query, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        _logger.LogInformation("Sucesso no logon.");
        return Ok(result);
    }

    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand query)
    {
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema no logout. {@request}; {@errorType} : {@error}", query, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        _logger.LogInformation("Sucesso no logout.");
        return Ok(result);
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema no refreshToken. {@command}; {@errorType} : {@error}", command, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        _logger.LogInformation("Sucesso no refreshToken.");
        return Ok(result);
    }
}
