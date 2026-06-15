using Application.DTO;
using Application.DTO.Travel;
using MediatR;

namespace Application.UseCases.Travels.ListTravel;

public class ListTravelQuery : IRequest<Result<List<TravelDTO>>>
{
    public Guid VehicleId { get; set; }
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 10;
}
