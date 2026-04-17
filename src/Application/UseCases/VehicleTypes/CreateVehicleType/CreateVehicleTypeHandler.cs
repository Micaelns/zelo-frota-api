using Application.DTO;
using Application.UseCases.Travels.EndsTravel;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.VehicleTypes.CreateVehicleType;

public class CreateVehicleTypeHandler(IVehicleTypeRepository vehicleTypeRepository, ILogger<CreateVehicleTypeHandler> logger) : IRequestHandler<CreateVehicleTypeCommand, Result<Guid>>
{
    private readonly IVehicleTypeRepository _repository = vehicleTypeRepository;
    private readonly ILogger<CreateVehicleTypeHandler> _logger = logger;

    public async Task<Result<Guid>> Handle(
        CreateVehicleTypeCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicleType = new VehicleType(command.Name);

            await _repository.AddAsync(vehicleType);

            _logger.LogInformation("Tipo de veículo foi cadastrado com sucesso.");
            return Result<Guid>.Success(vehicleType.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de cadastra tipo de Veiculo. {@command} {@error}", command, ex.Message);
            return Result<Guid>.Failure(ex.Message);
        }
    }
}
