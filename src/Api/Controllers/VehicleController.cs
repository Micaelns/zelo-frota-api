using Api.Requests;
using Api.Requests.Vehicles;
using Application.UseCases.Travels.EndsTravel;
using Application.UseCases.Travels.ListTravel;
using Application.UseCases.Travels.StartTravel;
using Application.UseCases.Vehicles.CreateVehicle;
using Application.UseCases.Vehicles.ListVehicle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ListVehicleQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVehicleCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Created("", result.Value);
    }

    [HttpPost]
    [Route("{vehicleId}/start-travel")]
    public async Task<IActionResult> StartTravel([FromRoute] Guid vehicleId, [FromBody] StartTravelRequest request)
    {
        var command = new StartTravelCommand
        {
            VehicleId = vehicleId,
            DestinationId = request.DestinationId,
            WhenTravel = request.WhenTravel
        };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Created("", result.Value);
    }

    [HttpPost]
    [Route("{vehicleId}/ends-travel")]
    public async Task<IActionResult> EndsTravel([FromRoute] Guid vehicleId, [FromBody] EndsTravelRequest request)
    {
        var command = new EndsTravelCommand
        {
            VehicleId = vehicleId,
            FinishMileage = request.FinishMileage,
            FuelQTD = request.FuelQTD,
            WhenArrived = request.WhenArrived
        };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Created("", result.Value);
    }

    [HttpGet]
    [Route("{vehicleId}/travels")]
    public async Task<IActionResult> GetStartTravel([FromRoute] Guid vehicleId, [FromQuery] PaginateRequest request)
    {
        var command = new ListTravelQuery
        {
            VehicleId = vehicleId,
            Skip = request.Skip,
            Take = request.Take
        };

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }
}
