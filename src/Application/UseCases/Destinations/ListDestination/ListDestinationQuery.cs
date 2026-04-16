using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Destinations.ListDestination;

public class ListDestinationQuery : IRequest<Result<List<Destination>>>
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}
