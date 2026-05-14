using Application.DTO;
using Application.DTO.Travel;
using MediatR;

namespace Application.UseCases.Travels.StartTravel;

public class StartTravelCommand : IRequest<Result<TravelDTO>>
{
    public required Guid VehicleId { get; set; }
    public required Guid DestinationId { get; set; }
    public DateTime? WhenTravel { get; set; }
}
