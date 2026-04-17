using Application.DTO;
using Application.UseCases.Travels.EndsTravel;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Vehicles.ListVehicle;

public class ListVehicleHandler(IVehicleRepository repository, ILogger<ListVehicleHandler> logger) : IRequestHandler<ListVehicleQuery, Result<List<Vehicle>>>
{
    private readonly IVehicleRepository _repository = repository;
    private readonly ILogger<ListVehicleHandler> _logger = logger;

    public async Task<Result<List<Vehicle>>> Handle(
        ListVehicleQuery command,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicleList = await _repository.AllAsync(command.Skip, command.Take);

            _logger.LogInformation("Listagem de viagens foi finalizado com sucesso.");
            return Result<List<Vehicle>>.Success(vehicleList.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de listar viagens. {@error}", ex.Message);
            return Result<List<Vehicle>>.Failure(ex.Message);
        }
    }
}
