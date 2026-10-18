using Application.Contracts.Abstractions.Travels.Query;
using Application.DTO;
using Application.DTO.Travel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Vehicles.RankingMileage;

public class RankingMileageHandler(ITravelQuery travelQuery, ILogger<RankingMileageHandler> logger) : IRequestHandler<RankingMileageQuery, Result<List<VehicleMileageRankingDTO>>>
{
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly ILogger<RankingMileageHandler> _logger = logger;

    public async Task<Result<List<VehicleMileageRankingDTO>>> Handle(
        RankingMileageQuery query,
        CancellationToken cancellationToken)
    {
        try
        {
            var economyVehicleList = await _travelQuery.GetRankingMileageAsync(query.OrderByDescending , query.Take);

            _logger.LogInformation("Listagem do ranking de quilometragem percorrida foi finalizado com sucesso.");
            return Result<List<VehicleMileageRankingDTO>>.Success(economyVehicleList.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de listar ranking de quilometragem percorrida. {@error}", ex.Message);
            return Result<List<VehicleMileageRankingDTO>>.Failure(ex.Message);
        }
    }
}

