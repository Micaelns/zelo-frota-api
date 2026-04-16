using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Domain.ObjectValues;
using MediatR;

namespace Application.UseCases.Vehicles.CreateVehicle;

public class CreateVehicleHandler(IVehicleRepository repository, IVehicleTypeRepository vehicleTypeRepository) : IRequestHandler<CreateVehicleCommand, Result<Guid>>
{
    private readonly IVehicleRepository _repository = repository;
    private readonly IVehicleTypeRepository _vehicleTypeRepository = vehicleTypeRepository;

    public async Task<Result<Guid>> Handle(
        CreateVehicleCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var plate = new Plate(command.Plate);
            var exists = await _repository.GetByPlateAsync(plate.Value);

            if (exists is not null)
                return Result<Guid>.Failure("Veículo já cadastrado");

            var vehicleType = await _vehicleTypeRepository.FindAsync(command.Type);

            if (vehicleType is null)
                return Result<Guid>.Failure("Tipo de veículo não existe");

            var vehicle = new Vehicle(
                command.Type,
                plate,
                command.InitialMileage
            );

            await _repository.AddAsync(vehicle);

            return Result<Guid>.Success(vehicle.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(ex.Message);
        }
    }
}
