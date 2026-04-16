using Application.DTO;
using MediatR;

namespace Application.UseCases.VehicleTypes.CreateVehicleType;

public class CreateVehicleTypeCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = string.Empty;

}
