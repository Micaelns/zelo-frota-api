using Application.Contracts.Events;
using Application.Contracts.Messaging;
using Application.DTO;
using Application.UseCases.Travels.EndsTravel;
using Domain.Entities;
using Domain.Interfaces.Query;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Travels.StartTravel;

public class StartTravelHandler(IVehicleRepository vehicleRepository, IDestinationRepository destinationRepository, ITravelQuery travelQuery, IMessageProducer producer, ILogger<StartTravelHandler> logger) : IRequestHandler<StartTravelCommand, Result<Travel>>
{
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    private readonly IDestinationRepository _destinationRepository = destinationRepository;
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly IMessageProducer _producer = producer;
    private readonly ILogger<StartTravelHandler> _logger = logger;

    public async Task<Result<Travel>> Handle(
        StartTravelCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _vehicleRepository.FindAsync(command.VehicleId);

            if (vehicle is null)
            {
                _logger.LogError("Veículo {@VehicleId} não encontrado", command.VehicleId);
                return Result<Travel>.Failure("Veículo não encontrado");
            }

            var destination = await _destinationRepository.FindAsync(command.DestinationId);

            if (destination is null)
            {
                _logger.LogError("Destino{@command.DestinationId} não encontrado", command.DestinationId);
                return Result<Travel>.Failure("Destino não encontrado");
            }

            var hasOpenTravel = await _travelQuery.HasOpenTravelInVehicleAsync(command.VehicleId);
            var travel = vehicle.StartTravel(command.DestinationId, hasOpenTravel, command.WhenTravel);

            await _vehicleRepository.SaveChangesAsync();
            await Notify(travel);

            _logger.LogInformation("Viagem do veículo {@Plate} foi iniciada com sucesso.", vehicle.Plate);
            return Result<Travel>.Success(travel);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de iniciar viagem. {@error}", ex.Message);
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
