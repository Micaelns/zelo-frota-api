using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Domain.ObjectValues;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Vehicles.CreateVehicle;

public class CreateVehicleHandler(IVehicleRepository repository, IVehicleTypeRepository vehicleTypeRepository, ILogger<CreateVehicleHandler> logger) : IRequestHandler<CreateVehicleCommand, Result<Guid>>
{
    private readonly IVehicleRepository _repository = repository;
    private readonly IVehicleTypeRepository _vehicleTypeRepository = vehicleTypeRepository;
    private readonly ILogger<CreateVehicleHandler> _logger = logger;

    public async Task<Result<Guid>> Handle(
        CreateVehicleCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var plate = new Plate(command.Plate);
            var exists = await _repository.GetByPlateAsync(plate.Value);

            if (exists is not null)
            {
                _logger.LogError("Veículo {@Plate} já cadastrado", command.Plate);
                return Result<Guid>.Failure("Veículo já cadastrado");
            }

            var vehicleType = await _vehicleTypeRepository.FindAsync(command.Type);

            if (vehicleType is null)
            {
                _logger.LogError("Tipo de veículo {@command.Type} não existe", command.Type);
                return Result<Guid>.Failure("Tipo de veículo não existe");
            }

            var vehicle = new Vehicle(
                command.Type,
                plate,
                command.InitialMileage
            );

            await _repository.AddAsync(vehicle);

            _logger.LogInformation("Veículo {@command.Plate} foi cadastrado com sucesso.", command.Plate);
            return Result<Guid>.Success(vehicle.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de cadastrar veiculo. {@command} {@error}", command, ex.Message);
            return Result<Guid>.Failure(ex.Message);
        }
    }
}
