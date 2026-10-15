using Application.DTO;
using Application.UseCases.Destinations.CreateDestination;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Drivers.CreateDriver;

public class CreateDriverHandler(IDriverRepository repository, ILogger<CreateDriverHandler> logger) : IRequestHandler<CreateDriverCommand, Result<Guid>>
{
    private readonly IDriverRepository _repository = repository;
    private readonly ILogger<CreateDriverHandler> _logger = logger;

    public async Task<Result<Guid>> Handle(
        CreateDriverCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var driver = new Driver(command.Name, command.Description, command.DriverLicense);

            await _repository.AddAsync(driver);

            _logger.LogInformation("Motorista foi cadastrado com sucesso.");
            return Result<Guid>.Success(driver.Id);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("Erro no processo de cadastrar motorista. {@command} {@error}", command, ex.Message);
            return Result<Guid>.Failure(ex.Message,ErrorType.Validation);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de cadastrar motorista. {@command} {@error}", command, ex.Message);
            return Result<Guid>.Failure(ex.Message);
        }
    }
}
