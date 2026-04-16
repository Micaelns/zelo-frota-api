using Application.Contracts.Events;
using Application.DTO;
using Application.Interfaces.Messaging;
using Domain.Entities;
using Domain.Interfaces.Query;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.Vehicles.StartTravels;

public class StartTravelHandler(IVehicleRepository vehicleRepository, IDestinationRepository destinationRepository, ITravelQuery travelQuery, IMessageProducer producer) : IRequestHandler<StartTravelCommand, Result<Travel>>
{
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    private readonly IDestinationRepository _destinationRepository = destinationRepository;
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly IMessageProducer _producer = producer;

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

            var hasOpenTravel = await _travelQuery.HasOpenTravelInVehicleAsync(command.VehicleId);
            var travel = vehicle.StartTravel(command.DestinationId, hasOpenTravel, command.WhenTravel);

            await _vehicleRepository.SaveChangesAsync();
            await Notify(travel);

            return Result<Travel>.Success(travel);
        }
        catch (Exception ex)
        {
            return Result<Travel>.Failure(ex.Message);
        }
    }

    private async Task Notify(Travel travel)
    {
        await _producer.PublishAsync(new TravelStartedEvent()
        {
            TravelId = travel.Id,
            VehicleId = travel.VehicleId,
            DestinationId = travel.DestinationId,
            StartedMileage = travel.StartedMileage,
            Start = travel.Start
        });
    }
}
