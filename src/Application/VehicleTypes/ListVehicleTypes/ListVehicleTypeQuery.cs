using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.VehicleTypes.ListVehicleTypes;

public class ListVehicleTypeQuery : IRequest<Result<List<VehicleType>>>
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}
