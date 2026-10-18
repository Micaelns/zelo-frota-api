using Api.Helper;
using Application.Security;
using Application.UseCases.Destinations.CreateDestination;
using Application.UseCases.Destinations.ListDestination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DestinationController(IMediator mediator, ILogger<DestinationController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<DestinationController> _logger = logger;

    [HttpGet]
    [Authorize(Policy = Permission.ListDestination)]
    public async Task<IActionResult> Index([FromQuery] ListDestinationQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema na listagem de destinos. {@request} ; {@errorType} : {@error}", query, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        _logger.LogInformation("Sucesso na listagem de destinos.");
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = Permission.CreateDestination)]
    public async Task<IActionResult> Create(CreateDestinationCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema na criação de destino. {@command} ; {@errorType} : {@error}", command, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        _logger.LogInformation("Sucesso na criação de destino.");
        return Created("", result);
    }
}