using Application.Contracts.Abstractions;
using Application.Contracts.Abstractions.Travels.Query;
using Application.Contracts.Events;
using Application.DTO;
using Application.DTO.Travel;
using Application.Mappers;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Travels.StartTravel;

public class StartTravelHandler(IVehicleRepository vehicleRepository, IDestinationRepository destinationRepository, ITravelQuery travelQuery, IMessageProducer producer, ILogger<StartTravelHandler> logger) : IRequestHandler<StartTravelCommand, Result<TravelDTO>>
{
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    private readonly IDestinationRepository _destinationRepository = destinationRepository;
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly IMessageProducer _producer = producer;
    private readonly ILogger<StartTravelHandler> _logger = logger;

    public async Task<Result<TravelDTO>> Handle(
        StartTravelCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _vehicleRepository.FindAsync(command.VehicleId);

            if (vehicle is null)
            {
                _logger.LogError("Veículo {@VehicleId} não encontrado", command.VehicleId);
                return Result<TravelDTO>.Failure("Veículo não encontrado");
            }

            var destination = await _destinationRepository.FindAsync(command.DestinationId);

            if (destination is null)
            {
                _logger.LogError("Destino{@command.DestinationId} não encontrado", command.DestinationId);
                return Result<TravelDTO>.Failure("Destino não encontrado");
            }

            var hasOpenTravel = await _travelQuery.HasOpenTravelInVehicleAsync(command.VehicleId);
            var travel = vehicle.StartTravel(command.DestinationId, hasOpenTravel, command.WhenTravel);

            await _vehicleRepository.SaveChangesAsync();

            travel = await _travelQuery.FindAsync(travel.Id);

            await Notify(travel!, cancellationToken);

            _logger.LogInformation("Viagem do veículo {@Plate} foi iniciada com sucesso.", vehicle.Plate);
            return Result<TravelDTO>.Success(TravelMapper.ToTravelDTO(travel));
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de iniciar viagem. {@error}", ex.Message);
            return Result<TravelDTO>.Failure(ex.Message);
        }
    }

    private async Task Notify(Travel travel, CancellationToken cancellationToken)
    {
        await _producer.PublishAsync(new TravelStartedEvent()
        {
            TravelId = travel.Id,
            VehicleId = travel.VehicleId,
            DestinationId = travel.DestinationId,
            StartedMileage = travel.StartedMileage,
            Start = travel.Start
        }, cancellationToken);
    }
}
