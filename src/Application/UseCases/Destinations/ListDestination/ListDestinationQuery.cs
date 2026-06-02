using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Destinations.ListDestination;

public class ListDestinationQuery : IRequest<Result<List<Destination>>>
{
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 10;
}
