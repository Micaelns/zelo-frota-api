using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.UseCases.Destinations.ListDestination;

public class ListDestinationHandle(IDestinationRepository repository) : IRequestHandler<ListDestinationQuery, Result<List<Destination>>>
{
    private readonly IDestinationRepository _repository = repository;
    public async Task<Result<List<Destination>>> Handle(
        ListDestinationQuery command,
        CancellationToken cancellationToken)
    {
        try
        {
            var DestinationList = await _repository.AllAsync(command.Skip, command.Take);

            return Result<List<Destination>>.Success(DestinationList.ToList());
        }
        catch (Exception ex)
        {
            return Result<List<Destination>>.Failure(ex.Message);
        }
    }
}
