using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.VehicleTypes.DeleteVehicleType;

public class DeleteVehicleTypeCommand : IRequest<Result<VehicleType>>
{
    public required Guid Id { get; set; }
}
