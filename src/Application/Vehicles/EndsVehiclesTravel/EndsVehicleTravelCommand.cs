using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Vehicles.EndsVehiclesTravel;

public class EndsVehicleTravelCommand : IRequest<Result<Travel>>
{
    public required Guid TravelId { get; set; }
    public required Guid VehicleId { get; set; }
    public int FinishMileage { get; set; }
    public float FuelQTD { get; set; }
    public DateTime? whenArrived { get; set; }
}
