using Application.Contracts.Abstractions.Travels.Query;
using Application.DTO;
using Application.DTO.Travel;
using Application.Mappers;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Travels.ListTravel;

public class ListTravelHandler(ITravelQuery travelQuery, IVehicleRepository vehicleRepository, ILogger<ListTravelHandler> logger) : IRequestHandler<ListTravelQuery, Result<List<TravelDTO>>>
{
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    private readonly ILogger<ListTravelHandler> _logger = logger;
    public async Task<Result<List<TravelDTO>>> Handle(
        ListTravelQuery command,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _vehicleRepository.FindAsync(command.VehicleId);

            if (vehicle is null)
            {
                _logger.LogError("Veículo {@VehicleId} não encontrado", command.VehicleId);
                return Result<List<TravelDTO>>.Failure("Veículo não encontrado");
            }

            var vehicleTravelList = await _travelQuery.GetTravelsByVehicleAsync(command.VehicleId,command.Skip, command.Take);

            _logger.LogInformation("Lista de viagens do veículo {@Plate} foi finalizada com sucesso.", vehicle.Plate);
            return Result<List<TravelDTO>>.Success(TravelMapper.ToListTravelDTO(vehicleTravelList.ToList()));
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de listar viagens do veículo {@command.VehicleId}. {@error}", command.VehicleId, ex.Message);
            return Result<List<TravelDTO>>.Failure(ex.Message);
        }
    }
}
