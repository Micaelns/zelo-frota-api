using Application.DTO;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces.Repository;
using Domain.ObjectValues;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Destinations.CreateDestination;

public class CreateDestinationHandler(IDestinationRepository repository, ILogger<CreateDestinationHandler> logger) : IRequestHandler<CreateDestinationCommand, Result<Guid>>
{
    private readonly IDestinationRepository _repository = repository;
    private readonly ILogger<CreateDestinationHandler> _logger = logger;

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
            {
                _logger.LogWarning("CEP {@CEP} já cadastrado ", command.ZipCode);
                return Result<Guid>.Failure("CEP já cadastrado");
            }

            var destination = Destination.CreateDestination(zipCode, command.Address, command.Neighborhood, command.Locality, command.City, uf);
            await _repository.AddAsync(destination);
            _logger.LogInformation("Sucesso na criação do destino. {@command}", command);
            return Result<Guid>.Success(destination.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de criação de destino. {@error}", ex.Message);
            return Result<Guid>.Failure(ex.Message);
        }
    }
}
