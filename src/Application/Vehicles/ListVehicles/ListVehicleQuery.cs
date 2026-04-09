using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Vehicles.ListVehicles;

public class ListVehicleQuery : IRequest<Result<List<Vehicle>>>
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}
