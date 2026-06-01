using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.VehicleTypes.UpdateVehicleType;

public class UpdateVehicleTypeHandler(IVehicleTypeRepository vehicleTypeRepository, ILogger<UpdateVehicleTypeHandler> logger) : IRequestHandler<UpdateVehicleTypeCommand, Result<VehicleType>>
{
    private readonly IVehicleTypeRepository _repository = vehicleTypeRepository;
    private readonly ILogger<UpdateVehicleTypeHandler> _logger = logger;

    public async Task<Result<VehicleType>> Handle(
        UpdateVehicleTypeCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicleType = await _repository.FindAsync(command.Id);

            if (vehicleType is null)
            {
                _logger.LogError("Tipo de veículo {@id} não existe", command.Id);
                return Result<VehicleType>.Failure("Tipo de veículo não existe", ErrorType.Validation);
            }

            vehicleType.Name = command.Name;

            await _repository.UpdateAsync(vehicleType);

            _logger.LogInformation("Tipo de veículo foi editado com sucesso.");
            return Result<VehicleType>.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de editar tipo de Veiculo. {@command} {@error}", command, ex.Message);
            return Result<VehicleType>.Failure(ex.Message, ErrorType.Internal);
        }
    }
}
