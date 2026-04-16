using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Travels.EndsTravel;

public class EndsTravelCommand : IRequest<Result<Travel>>
{
    public required Guid VehicleId { get; set; }
    public int FinishMileage { get; set; }
    public float FuelQTD { get; set; }
    public DateTime? WhenArrived { get; set; }
}
