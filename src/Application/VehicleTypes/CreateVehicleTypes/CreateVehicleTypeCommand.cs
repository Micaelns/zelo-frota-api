using Application.DTO;
using MediatR;

namespace Application.VehicleTypes.CreateVehicleTypes;

public class CreateVehicleTypeCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = string.Empty;

}
