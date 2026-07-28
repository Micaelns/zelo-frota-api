using Application.Contracts.Abstractions.Travels.Query;
using Application.DTO;
using Application.DTO.Travel;

using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Vehicles.MileageRanking;

public class MileageRankingHandler(ITravelQuery travelQuery, ILogger<MileageRankingHandler> logger) : IRequestHandler<MileageRankingQuery, Result<List<VehicleMileageRankingDTO>>>
{
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly ILogger<MileageRankingHandler> _logger = logger;

    public async Task<Result<List<VehicleMileageRankingDTO>>> Handle(
        MileageRankingQuery command,
        CancellationToken cancellationToken)
    {
        try
        {
            var economyVehicleList = await _travelQuery.GetMileageHankingAsync(command.OrderByDescending ,command.Skip, command.Take);

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

