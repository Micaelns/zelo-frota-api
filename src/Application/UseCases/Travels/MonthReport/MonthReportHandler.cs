using Application.Contracts.Events;
using Application.Contracts.Messaging;
using Application.DTO;
using Domain.Interfaces.Query;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Travels.MonthReport;

public class MonthReportHandler(IVehicleRepository vehicleRepository, IDestinationRepository destinationRepository, ITravelQuery travelQuery, IMessageProducer producer, ILogger<MonthReportHandler> logger) : IRequestHandler<MonthReportCommand, Result<string>>
{
    private readonly IVehicleRepository _vehicleRepository = vehicleRepository;
    private readonly IDestinationRepository _destinationRepository = destinationRepository;
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly IMessageProducer _producer = producer;
    private readonly ILogger<MonthReportHandler> _logger = logger;

    public async Task<Result<string>> Handle(
        MonthReportCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            if (command.VehicleId != null)
            {
                var vehicle = await _vehicleRepository.FindAsync((Guid)command.VehicleId);
                if (vehicle is null)
                {
                    _logger.LogError("Veículo {@VehicleId} não encontrado", command.VehicleId);
                    return Result<string>.Failure("Veículo não encontrado");
                }
            }

            if (command.DestinationId != null)
            {
                var destination = await _destinationRepository.FindAsync((Guid)command.DestinationId);

                if (destination is null)
                {
                    _logger.LogError("Destino{@command.DestinationId} não encontrado", command.DestinationId);
                    return Result<string>.Failure("Destino não encontrado");
                }
            }

            await Notify(command);

            _logger.LogInformation("Solicitação de relatório {@command} enviado com sucesso.", command);
            return Result<string>.Success("Solicitação de relatório enviado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de solicitação de relatório. {@error}", ex.Message);
            return Result<string>.Failure(ex.Message);
        }
    }

    private async Task Notify(MonthReportCommand command)
    {
        await _producer.PublishAsync(new TravelReportEvent()
        {
            VehicleId = command.VehicleId,
            DestinationId = command.DestinationId,
            MonthTravel = command.MonthTravel,
            YearTravel = command.YearTravel
        });
    }
}
