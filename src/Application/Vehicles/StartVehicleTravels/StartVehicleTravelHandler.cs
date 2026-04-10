using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.Vehicles.StartVehicleTravels;

public class StartVehicleTravelHandler(IVehicleRepository vehicleRepository, ITravelRepository travelRepository, IDestinationRepository destinationRepository) : IRequestHandler<StartVehicleTravelCommand, Result<Travel>>
{
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    private readonly ITravelRepository _travelRepository = travelRepository;
    private readonly IDestinationRepository _destinationRepository = destinationRepository;

    public async Task<Result<Travel>> Handle(
        StartVehicleTravelCommand command,
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

            var travel = new Travel(command.VehicleId, command.DestinationId);
            travel.Starts(command.CurrentMileage, command.WhenTravel);

            await _travelRepository.AddAsync(travel);

            return Result<Travel>.Success(travel);
        }
        catch (Exception ex)
        {
            return Result<Travel>.Failure(ex.Message);
        }
    }
}
