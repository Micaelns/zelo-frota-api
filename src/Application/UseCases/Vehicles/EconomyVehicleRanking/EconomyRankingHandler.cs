using Application.Contracts.Abstractions.Travels.Query;
using Application.DTO;
using Application.DTO.Travel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Vehicles.EconomyVehicleRanking;

public class EconomyRankingHandler(ITravelQuery travelQuery, ILogger<EconomyRankingHandler> logger) : IRequestHandler<EconomyRankingQuery, Result<List<VehicleEconomyDto>>>
{
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly ILogger<EconomyRankingHandler> _logger = logger;

    public async Task<Result<List<VehicleEconomyDto>>> Handle(
        EconomyRankingQuery command,
        CancellationToken cancellationToken)
    {
        try
        {
            var economyVehicleList = await _travelQuery.GetHankingVehicleEconomyAsync(command.Skip, command.Take);

            _logger.LogInformation("Listagem do ranking de veiculos mais econômicos foi finalizado com sucesso.");
            return Result<List<VehicleEconomyDto>>.Success(economyVehicleList.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro no processo de listar ranking de veiculos mais econômicos. {@error}", ex.Message);
            return Result<List<VehicleEconomyDto>>.Failure(ex.Message);
        }
    }
}
