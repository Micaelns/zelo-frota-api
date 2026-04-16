using Application.DTO;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces.Repository;
using Domain.ObjectValues;
using MediatR;

namespace Application.UseCases.Destinations.CreateDestination;

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
            var zipCode = new ZipCode(command.ZipCode);

            var exists = await _repository.GetByZipCodeAsync(zipCode.Value);

            if (exists is not null)
                return Result<Guid>.Failure("CEP já cadastrado");

            var destination = Destination.CreateDestination(zipCode, command.Address, command.Neighborhood, command.Locality, command.City, uf);
            await _repository.AddAsync(destination);
            return Result<Guid>.Success(destination.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(ex.Message);
        }
    }
}
