using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.VehicleTypes.CreateVehicleTypes;

public class CreateVehicleTypeHandler(IVehicleTypeRepository vehicleTypeRepository) : IRequestHandler<CreateVehicleTypeCommand, Result<Guid>>
{
    private readonly IVehicleTypeRepository _repository = vehicleTypeRepository;

    public async Task<Result<Guid>> Handle(
        CreateVehicleTypeCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var vehicleType = new VehicleType(command.Name);

            await _repository.AddAsync(vehicleType);

            return Result<Guid>.Success(vehicleType.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(ex.Message);
        }
    }
}
