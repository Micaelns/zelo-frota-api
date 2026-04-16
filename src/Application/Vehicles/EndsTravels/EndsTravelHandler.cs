using Application.Contracts.Events;
using Application.DTO;
using Application.Interfaces.Messaging;
using Domain.Entities;
using Domain.Interfaces.Query;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.Vehicles.EndsTravels;

public class EndsTravelHandler(IVehicleRepository repository, ITravelQuery travelQuery, IMessageProducer producer) : IRequestHandler<EndsTravelCommand, Result<Travel>>
{
    private readonly IVehicleRepository _repository = repository;
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly IMessageProducer _producer = producer;


    public async Task<Result<Travel>> Handle(
        EndsTravelCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _repository.FindAsync(command.VehicleId);

            if (vehicle is null)
                return Result<Travel>.Failure("Veículo não encontrado");

            var travel = await _travelQuery.GetOpenTravelInVehicleAsync(command.VehicleId);

            if (travel is null)
                return Result<Travel>.Failure("Não existe viagem em andamento");

            vehicle.EndTravel(travel, command.FinishMileage, command.FuelQTD, command.WhenArrived);

            await _repository.SaveChangesAsync();
            await Notify(travel, command);

            return Result<Travel>.Success(travel);
        }
        catch (Exception ex)
        {
            return Result<Travel>.Failure(ex.Message);
        }
    }

    private async Task Notify(Travel travel, EndsTravelCommand command)
    {
        await _producer.PublishAsync(new TravelEndedEvent()
        {
            TravelId = travel.Id,
            VehicleId = travel.VehicleId,
            DestinationId = travel.DestinationId,
            StartedMileage = travel.StartedMileage,
            FinishedMileage = travel.FinishedMileage,
            Autonomy = travel.Autonomy,
            Start = travel.Start,
            End = travel.End
        });

    }
}
