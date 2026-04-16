using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Travels.StartTravel;

public class StartTravelCommand : IRequest<Result<Travel>>
{
    public required Guid VehicleId { get; set; }
    public required Guid DestinationId { get; set; }
    public DateTime? WhenTravel { get; set; }
}
