using Application.Destinations.CreateDestinations;
using Application.Destinations.ListDestinations;
using Application.VehicleTypes.CreateVehicleTypes;
using Application.VehicleTypes.ListVehicleTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DestinationController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ListDestinationQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateDestinationCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Created("", result.Value);
    }
}