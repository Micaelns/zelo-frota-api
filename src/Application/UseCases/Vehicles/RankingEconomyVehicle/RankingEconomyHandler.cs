using Application.Contracts.Abstractions.Travels.Query;
using Application.DTO;
using Application.DTO.Travel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Vehicles.RankingEconomyVehicle;

public class RankingEconomyHandler(ITravelQuery travelQuery, ILogger<RankingEconomyHandler> logger) : IRequestHandler<RankingEconomyQuery, Result<List<VehicleEconomyDto>>>
{
    private readonly ITravelQuery _travelQuery = travelQuery;
    private readonly ILogger<RankingEconomyHandler> _logger = logger;

    public async Task<Result<List<VehicleEconomyDto>>> Handle(
        RankingEconomyQuery command,
        CancellationToken cancellationToken)
    {
        try
        {
            var economyVehicleList = await _travelQuery.GetRankingVehicleEconomyAsync(command.OrderByDescending, command.Take);

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
