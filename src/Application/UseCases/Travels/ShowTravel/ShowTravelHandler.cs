using Application.Contracts.Abstractions.Travels.Query;
using Application.DTO;
using Application.DTO.Travel;
using Application.Mappers;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Travels.ShowTravel;

public class ShowTravelHandler(ITravelQuery travelQuery, ILogger<ShowTravelHandler> logger) : IRequestHandler<ShowTravelQuery, Result<TravelDTO>>
{
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly ILogger<ShowTravelHandler> _logger = logger;

    public async Task<Result<TravelDTO>> Handle(
        ShowTravelQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var travel = await _travelQuery.FindAsync(query.Id);
            if (travel is null)
            {
                _logger.LogError("Viagem {@id} não localizada", query.Id);
                return Result<TravelDTO>.Failure("Viagem não localizada", ErrorType.Validation);
            }
            _logger.LogInformation("Retornado viagem com sucesso.");
            return Result<TravelDTO>.Success(TravelMapper.ToTravelDTO(travel));
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de procurar viagem. {@query} {@error}", query, ex.Message);
            return Result<TravelDTO>.Failure(ex.Message);
        }
    }
}
