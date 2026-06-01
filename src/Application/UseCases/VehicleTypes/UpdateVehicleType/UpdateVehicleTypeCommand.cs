using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.VehicleTypes.UpdateVehicleType;

public class UpdateVehicleTypeCommand : IRequest<Result<VehicleType>>
{
    public required Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
