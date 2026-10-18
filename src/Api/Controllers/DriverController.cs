using Application.Security;
using Application.UseCases.Drivers.CreateDriver;
using Application.UseCases.Drivers.ListDriver;
using Application.UseCases.Drivers.ShowDriver;
using Application.UseCases.Travels.RankingTravelDriver;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DriverController(IMediator mediator, ILogger<DriverController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<DriverController> _logger = logger;

    [HttpGet]
    [Authorize(Policy = Permission.ListDriver)]
    public async Task<IActionResult> Index([FromQuery] ListDriverQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema na listagem de motoristas. BadRequest: {@request} : {@error}", query, result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Sucesso na listagem de motoristas.");
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = Permission.CreateDriver)]
    public async Task<IActionResult> Create(CreateDriverCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema na criação de motorista. {@command} {@error}", command, result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Sucesso na criação de motorista. Result: {@result}", result.Value);
        return Created("", result.Value);
    }

    [HttpGet]
    [Route("{driverId}")]
    [Authorize(Policy = Permission.FindDriver)]
    public async Task<IActionResult> Find([FromRoute] Guid driverId)
    {
        var query = new ShowDriverQuery
        {
            Id = driverId
        };

        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema ao encontrar motorista. BadRequest: {@query} : {@error}", query, result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Sucesso ao encontrar motoristar.");
        return Ok(result);
    }

    [HttpGet]
    [Route("ranking")]
    [Authorize(Policy = Permission.RankingTravelDriver)]
    public async Task<IActionResult> RankingTravelDriver([FromRoute] RankingTravelDriverQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("Problema ao listar Ranking de motoristas. BadRequest: {@query} : {@error}", query, result.Error);
            return BadRequest(result.Error);
        }

        _logger.LogInformation("Sucesso ao listar Ranking de motoristas.");
        return Ok(result);
    }
}
