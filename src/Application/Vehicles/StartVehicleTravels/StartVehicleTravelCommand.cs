using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Vehicles.StartVehicleTravels;

public class StartVehicleTravelCommand : IRequest<Result<Travel>>
{
    public required Guid VehicleId { get; set; }
    public required Guid DestinationId { get; set; }
    public int CurrentMileage { get; set; } = 0;
    public DateTime? WhenTravel { get; set; }
}
