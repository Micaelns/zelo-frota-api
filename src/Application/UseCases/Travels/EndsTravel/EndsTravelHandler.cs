using Application.Contracts.Abstractions;
using Application.Contracts.Events;
using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Query;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Travels.EndsTravel;

public class EndsTravelHandler(IVehicleRepository repository, ITravelQuery travelQuery, IMessageProducer producer, ILogger<EndsTravelHandler> logger) : IRequestHandler<EndsTravelCommand, Result<Travel>>
{
    private readonly IVehicleRepository _repository = repository;
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly IMessageProducer _producer = producer;
    private readonly ILogger<EndsTravelHandler> _logger = logger;


    public async Task<Result<Travel>> Handle(
        EndsTravelCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicle = await _repository.FindAsync(command.VehicleId);

            if (vehicle is null)
            {
                _logger.LogError("Veículo {@VehicleId} não encontrado", command.VehicleId);
                return Result<Travel>.Failure("Veículo não encontrado");
            }

            var travel = await _travelQuery.GetOpenTravelInVehicleAsync(command.VehicleId);

            if (travel is null)
            {
                _logger.LogError("Não existe viagem em andamento para {@VehicleId} ", command.VehicleId);
                return Result<Travel>.Failure("Não existe viagem em andamento");
            }

            vehicle.EndTravel(travel, command.FinishMileage, command.FuelQTD, command.WhenArrived);

            await _repository.SaveChangesAsync();
            await Notify(travel, command, cancellationToken);

            _logger.LogInformation("Viagem do veículo {@Plate} foi finalizado com sucesso.", vehicle.Plate);
            return Result<Travel>.Success(travel);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de finalizar viagem. {@command} {@error}", command, ex.Message);
            return Result<Travel>.Failure(ex.Message);
        }
    }

    private async Task Notify(Travel travel, EndsTravelCommand command, CancellationToken cancellationToken)
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
        }, cancellationToken);

    }
}
