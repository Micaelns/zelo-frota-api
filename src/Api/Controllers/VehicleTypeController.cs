using Application.UseCases.VehicleTypes.CreateVehicleType;
using Application.UseCases.VehicleTypes.ListVehicleType;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleTypeController(IMediator mediator, ILogger<VehicleTypeController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<VehicleTypeController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ListVehicleTypeQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema na listagem de tipo de veículo. BadRequest: {@request} : {@error}", query, result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Sucesso na listagem de tipo de veículo.");
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVehicleTypeCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema na Criação de tipo de veículo. {@command} {@error}", command, result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Sucesso na criação de tipo de veículo. Result: {@result}", result.Value);

        return Created("", result.Value);
    }
}
