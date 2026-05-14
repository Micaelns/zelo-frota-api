using Application.DTO;
using Application.DTO.Travel;
using MediatR;

namespace Application.UseCases.Travels.ListTravel;

public class ListTravelQuery : IRequest<Result<List<TravelDTO>>>
{
    public required Guid VehicleId { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}
