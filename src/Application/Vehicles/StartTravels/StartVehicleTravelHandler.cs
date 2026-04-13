using Application.DTO;
using Application.Vehicles.StartTravels;
using Domain.Entities;
using Domain.Interfaces.Query;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.Vehicles.StartVehicleTravels;

public class StartVehicleTravelHandler(IVehicleRepository vehicleRepository, IDestinationRepository destinationRepository, ITravelQuery travelQuery) : IRequestHandler<StartTravelCommand, Result<Travel>>
{
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    private readonly IDestinationRepository _destinationRepository = destinationRepository;
    private readonly ITravelQuery _travelQuery = travelQuery;

    public async Task<Result<Travel>> Handle(
        StartTravelCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _vehicleRepository.FindAsync(command.VehicleId);

            if (vehicle is null)
                return Result<Travel>.Failure("Veículo não encontrado");

            var destination = await _destinationRepository.FindAsync(command.DestinationId);

            if (destination is null)
                return Result<Travel>.Failure("Destino não encontrado");

            var hasOpenTravel = await _travelQuery.HasOpenTravelInVehicle(command.VehicleId);
            var travel = vehicle.StartTravel(command.DestinationId, hasOpenTravel, command.WhenTravel);

            await _vehicleRepository.SaveChangesAsync();

            return Result<Travel>.Success(travel);
        }
        catch (Exception ex)
        {
            return Result<Travel>.Failure(ex.Message);
        }
    }
}
