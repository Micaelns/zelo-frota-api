using Application.DTO;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces.Repository;
using MediatR;

namespace Application.Destinations.CreateDestinations;

public class CreateDestinationHandle(IDestinationRepository repository) : IRequestHandler<CreateDestinationCommand, Result<Guid>>
{
    private readonly IDestinationRepository _repository = repository;

    public async Task<Result<Guid>> Handle(
        CreateDestinationCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var uf = (UF)Enum.Parse(typeof(UF), command.Uf, true);
            var destination = Destination.CreateDestination(command.ZipCode, command.Address, command.Neighborhood, command.Locality, command.City, uf);
            await _repository.AddAsync(destination);
            return Result<Guid>.Success(destination.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(ex.Message);
        }
    }
}
