using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.VehicleTypes.DeleteVehicleType;

public class DeleteVehicleTypeHandler(IVehicleTypeRepository vehicleTypeRepository, ILogger<DeleteVehicleTypeHandler> logger) : IRequestHandler<DeleteVehicleTypeCommand, Result<VehicleType>>
{
    private readonly IVehicleTypeRepository _repository = vehicleTypeRepository;
    private readonly ILogger<DeleteVehicleTypeHandler> _logger = logger;

    public async Task<Result<VehicleType>> Handle(
        DeleteVehicleTypeCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            await _repository.DeleteLogicalAsync(command.Id);

            _logger.LogInformation("Tipo de veículo foi deletado com sucesso.");
            return Result<VehicleType>.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de deletar tipo de Veiculo. {@command} {@error}", command, ex.Message);
            return Result<VehicleType>.Failure(ex.Message, ErrorType.Internal);
        }
    }
}
