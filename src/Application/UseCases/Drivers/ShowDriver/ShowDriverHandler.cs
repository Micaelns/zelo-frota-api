using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Drivers.ShowDriver;

public class ShowDriverHandler(IDriverRepository repository, ILogger<ShowDriverHandler> logger) : IRequestHandler<ShowDriverQuery, Result<Driver>>
{
    private readonly IDriverRepository _repository = repository;
    private readonly ILogger<ShowDriverHandler> _logger = logger;

    public async Task<Result<Driver>> Handle(
        ShowDriverQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var driver = await _repository.FindAsync(query.Id);
            if (driver is null)
            {
                _logger.LogWarning("O Motorista informado não existe. {@query}", query);
                throw new ArgumentException("O Motorista informado não existe");
            }
            return Result<Driver>.Success(driver);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de consultar motorista. {@query} {@error}", query, ex.Message);
            return Result<Driver>.Failure(ex.Message);
        }
    }
}
