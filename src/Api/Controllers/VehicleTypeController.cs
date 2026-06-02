using Api.Requests.VehicleTypes;
using Application.DTO;
using Application.UseCases.VehicleTypes.CreateVehicleType;
using Application.UseCases.VehicleTypes.DeleteVehicleType;
using Application.UseCases.VehicleTypes.ListVehicleType;
using Application.UseCases.VehicleTypes.ShowVehicleType;
using Application.UseCases.VehicleTypes.UpdateVehicleType;
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
            return BadRequest(result);
        }

        _logger.LogInformation("Sucesso na listagem de tipo de veículo.");
        return Ok(result);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Show([FromRoute] Guid id)
    {
        var query = new ShowVehicleTypeQuery() { Id = id };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema ao mostrar de tipo de veículo. {@command} {@error}", query, result.Error);
            if (result.ErrorType == ErrorType.Validation)
            {
                return StatusCode(400, result);
            }
            return BadRequest(result);
        }

        _logger.LogInformation("Sucesso de mostrar de tipo de veículo. Result: {@result}", result.Value);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVehicleTypeCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema na Criação de tipo de veículo. {@command} {@error}", command, result.Error);
            if (result.ErrorType == ErrorType.Validation)
            {
                return StatusCode(400, result);
            }
            return BadRequest(result);
        }

        _logger.LogInformation("Sucesso na criação de tipo de veículo. Result: {@result}", result.Value);

        return Created("", result.Value);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, VehicleTypeRequest request)
    {
        var command = new UpdateVehicleTypeCommand()
        {
            Id = id,
            Name = request.Name
        };

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema ao editar tipo de veículo. {@command} {@error}", command, result.Error);
            if (result.ErrorType == ErrorType.Validation)
            {
                return StatusCode(400, result);
            }
            return BadRequest(result);
        }

        _logger.LogInformation("Sucesso ao editar tipo de veículo. Result: {@result}", result.Value);

        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteVehicleTypeCommand() { Id = id };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema ao deletar tipo de veículo. {@command} {@error}", command, result.Error);
            if (result.ErrorType == ErrorType.Validation)
            {
                return StatusCode(400, result);
            }
            return BadRequest(result);
        }

        _logger.LogInformation("Sucesso ao deletar tipo de veículo. Result: {@result}", result.Value);

        return Ok();
    }
}
