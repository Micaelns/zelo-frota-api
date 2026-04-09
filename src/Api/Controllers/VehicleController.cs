using Application.Vehicles.CreateVehicles;
using Application.Vehicles.ListVehicles;
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
            
        if(!result.IsSuccess)
        {
            return BadRequest(result.Error);
        } 

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVehicleCommand command)
    {
        var result = await _mediator.Send(command);
            
        if(!result.IsSuccess)
        {
            return BadRequest(result.Error);
        } 

        return Created("", result.Value);
    }
}
