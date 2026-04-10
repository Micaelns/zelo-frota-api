using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Vehicles.ListVehicleTravels;

public class ListVehicleTravelQuery : IRequest<Result<List<Travel>>>
{
    public required Guid VehicleId { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}
