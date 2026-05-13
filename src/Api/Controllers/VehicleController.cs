using Api.Requests;
using Api.Requests.Vehicles;
using Application.UseCases.Travels.EndsTravel;
using Application.UseCases.Travels.ListTravel;
using Application.UseCases.Travels.MonthReport;
using Application.UseCases.Travels.StartTravel;
using Application.UseCases.Vehicles.CreateVehicle;
using Application.UseCases.Vehicles.ListVehicle;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleController(IMediator mediator, ILogger<VehicleController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<VehicleController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] ListVehicleQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema na listagem de veículo. BadRequest: {@request} : {@error}", query, result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Sucesso na listagem de veículo.");
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVehicleCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema na criação de veículo. {@command} {@error}", command, result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Sucesso na criação de veículo. Result: {@result}", result.Value);
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
            _logger.LogWarning("Problema ao iniciar viagem. {@command} {@error}", command, result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Sucesso ao iniciar viagem. {@command}", command);
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
            _logger.LogWarning("Problema na Criação da viagem. {@command} {@error}", command, result.Error);
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
            _logger.LogWarning("Problema ao listar viagens. BadRequest: {@command} : {@error}", command, result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Sucesso ao listar viagens.");
        return Ok(result);
    }

    [HttpPost]
    [Route("travels/reports")]
    public async Task<IActionResult> GetReportsTravel(MonthReportCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema ao Solicitar Relatório. BadRequest: {@command} : {@error}", command, result.Error);
            
            if (result.ErrorType == Application.DTO.ErrorType.Validation)
                return BadRequest(result.Error);

            return StatusCode(500, result.Error);
        }

        _logger.LogInformation("Sucesso ao solicitar relatório de viagens.");
        return Ok(result);
    }
}
