using Application.UseCases.Destinations.CreateDestination;
using Application.UseCases.Destinations.ListDestination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DestinationController(IMediator mediator, ILogger<VehicleController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<VehicleController> _logger = logger;

    [HttpGet]
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
    public async Task<IActionResult> Create(CreateDestinationCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema na criação de destinos. {@command} {@error}", command, result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Sucesso na criação de destinos.");
        return Created("", result.Value);
    }
}