using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.Destinations.ListDestinations;

public class ListDestinationQuery : IRequest<Result<List<Destination>>>
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}
