using Application.Contracts.Abstractions.Travels.Query;
using Application.DTO;
using Application.DTO.Travel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Travels.RankingTravelDriver;

public class RankingTravelDriverHandler(ITravelQuery travelQuery, ILogger<RankingTravelDriverHandler> logger) : IRequestHandler<RankingTravelDriverQuery, Result<List<RankingTravelDriverDTO>>>
{
    public readonly ITravelQuery _travelQuery = travelQuery;
    public readonly ILogger<RankingTravelDriverHandler> _logger = logger;

    public async Task<Result<List<RankingTravelDriverDTO>>> Handle(
        RankingTravelDriverQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var driverRankingList = await _travelQuery.GetRankingTravelDriverAsync(query.OrderByDescending, query.Take);
            
            _logger.LogInformation("Ranking de motoristas foi feita com sucesso.");
            return Result<List<RankingTravelDriverDTO>>.Success(driverRankingList.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo o ranking de motoristas. {@error}", ex.Message);
            return Result<List<RankingTravelDriverDTO>>.Failure(ex.Message);
        }
    }
}
