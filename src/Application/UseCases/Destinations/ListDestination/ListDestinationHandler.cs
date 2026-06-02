using Application.DTO;
using Domain.Entities;
using Domain.Interfaces.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Destinations.ListDestination;

public class ListDestinationHandler(IDestinationRepository repository, ILogger<ListDestinationHandler> logger) : IRequestHandler<ListDestinationQuery, Result<List<Destination>>>
{
    private readonly IDestinationRepository _repository = repository;
    private readonly ILogger<ListDestinationHandler> _logger = logger;
    public async Task<Result<List<Destination>>> Handle(
        ListDestinationQuery command,
        CancellationToken cancellationToken)
    {
        try
        {
            var DestinationList = await _repository.AllAsync(command.Page, command.Take);
            var totalItems = await _repository.AllContAsync();
            var pagination = new Pagination(totalItems, command.Page, command.Take);

            _logger.LogInformation("Sucesso na listagem dos destinos. {@command}", command);

            return Result<List<Destination>>.Success(DestinationList.ToList(), pagination);
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de listagem de destino. {@error}", ex.Message);
            return Result<List<Destination>>.Failure(ex.Message);
        }
    }
}
