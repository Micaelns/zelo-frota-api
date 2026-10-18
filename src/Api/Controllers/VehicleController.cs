using Api.Helper;
using Api.Requests;
using Api.Requests.Vehicles;
using Application.Security;
using Application.UseCases.Travels.EndsTravel;
using Application.UseCases.Travels.ListTravel;
using Application.UseCases.Travels.MonthReport;
using Application.UseCases.Travels.ShowTravel;
using Application.UseCases.Travels.StartTravel;
using Application.UseCases.Vehicles.CreateVehicle;
using Application.UseCases.Vehicles.ListVehicle;
using Application.UseCases.Vehicles.RankingEconomyVehicle;
using Application.UseCases.Vehicles.RankingMileage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehicleController(IMediator mediator, ILogger<VehicleController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<VehicleController> _logger = logger;

    [HttpGet]
    [Authorize(Policy = Permission.ListVehicle)]
    public async Task<IActionResult> Index([FromQuery] ListVehicleQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema na listagem de veículo.  {@request} ; {@errorType} : {@error}", query, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        _logger.LogInformation("Sucesso na listagem de veículo.");
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = Permission.CreateVehicle)]
    public async Task<IActionResult> Create(CreateVehicleCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema na criação de veículo. {@command} ; {@errorType} : {@error}", command, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        _logger.LogInformation("Sucesso na criação de veículo. Result: {@result}", result.Value);
        return Created("", result);
    }

    [HttpPost]
    [Route("{vehicleId}/start-travel")]
    [Authorize(Policy = Permission.StartVehicleTravel)]
    public async Task<IActionResult> StartTravel([FromRoute] Guid vehicleId, [FromBody] StartTravelRequest request)
    {
        var command = new StartTravelCommand
        {
            VehicleId = vehicleId,
            DestinationId = request.DestinationId,
            DriverId = request.DriverId,
            WhenTravel = request.WhenTravel
        };
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema ao iniciar viagem. {@command} ; {@errorType} : {@error}", command, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        _logger.LogInformation("Sucesso ao iniciar viagem. {@command}", command);
        return Created("", result);
    }

    [HttpPost]
    [Route("{vehicleId}/ends-travel")]
    [Authorize(Policy = Permission.EndsVehicleTravel)]
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
            _logger.LogWarning("Problema ao finalizar viagem. {@command} ; {@errorType} : {@error}", command, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        return Created("", result.Value);
    }

    [HttpGet]
    [Route("{vehicleId}/travels")]
    [Authorize(Policy = Permission.ListVehicleTravel)]
    public async Task<IActionResult> GetStartTravel([FromRoute] Guid vehicleId, [FromQuery] PaginateRequest request)
    {
        var query = new ListTravelQuery
        {
            VehicleId = vehicleId,
            Page = request.Page,
            Take = request.Take
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema ao listar viagens. {@query} ; {@errorType} : {@error}", query, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        _logger.LogInformation("Sucesso ao listar viagens.");
        return Ok(result);
    }

    [HttpGet]
    [Route("travels/{travelId}")]
    [Authorize(Policy = Permission.FindVehicleTravel)]
    public async Task<IActionResult> GetTravel([FromRoute] Guid travelId)
    {
        var query = new ShowTravelQuery
        {
            Id = travelId
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema ao encontra viagem. {@query} ; {@errorType} : {@error}", query, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        _logger.LogInformation("Sucesso ao encontrar viagem.");
        return Ok(result);
    }

    [HttpGet]
    [Route("hanking/economy")]
    [Authorize(Policy = Permission.RankingEconomyVehicleTravel)]
    public async Task<IActionResult> GetRankingEconomy([FromQuery] RankingEconomyQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema ao solicitar ranking de veículos mais econômicos. {@query} ; {@errorType} : {@error}", query, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        _logger.LogInformation("Sucesso ao solicitar ranking de veículos mais econômicos.");
        return Ok(result);
    }

    [HttpGet]
    [Route("hanking/mileage")]
    [Authorize(Policy = Permission.RankingMilageVehicleTravel)]
    public async Task<IActionResult> GetRankingMilage([FromQuery] RankingMileageQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema ao solicitar ranking de quilometragem percorrida. {@query} ; {@errorType} : {@error}", query, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        _logger.LogInformation("Sucesso ao solicitar ranking de quilometragem percorrida.");
        return Ok(result);
    }

    [HttpPost]
    [Route("travels/reports")]
    [Authorize(Policy = Permission.ReportsVehicleTravel)]
    public async Task<IActionResult> GetReportsTravel(MonthReportCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema ao solicitar relatório. {@command} ; {@errorType} : {@error}", command, result.ErrorType, result.Error);
            return StatusCode(DecodeHelper.ErrorTypeToStatusCode(result.ErrorType), result);
        }

        _logger.LogInformation("Sucesso ao solicitar relatório de viagens.");
        return Ok(result);
    }
}
