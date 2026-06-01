using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.VehicleTypes.ShowVehicleType;

public class ShowVehicleTypeQuery : IRequest<Result<VehicleType>>
{
    public Guid Id { get; set; }
}
