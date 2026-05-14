using Application.DTO;
using Application.DTO.Travel;
using MediatR;

namespace Application.UseCases.Travels.EndsTravel;

public class EndsTravelCommand : IRequest<Result<TravelDTO>>
{
    public required Guid VehicleId { get; set; }
    public int FinishMileage { get; set; }
    public float FuelQTD { get; set; }
    public DateTime? WhenArrived { get; set; }
}
