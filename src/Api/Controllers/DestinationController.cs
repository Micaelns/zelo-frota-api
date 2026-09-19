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
            _logger.LogWarning("Problema na listagem de destinos. BadRequest: {@request} : {@error}", query, result.Error);
            return BadRequest(result.Error);
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
            _logger.LogWarning("Problema na criação de destino. {@command} {@error}", command, result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Sucesso na criação de destino.");
        return Created("", result.Value);
    }
}