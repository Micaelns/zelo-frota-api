using Application.DTO;
using Application.DTO.Travel;
using MediatR;

namespace Application.UseCases.Vehicles.EconomyVehicleRanking;

public class EconomyRankingQuery : IRequest<Result<List<VehicleEconomyDto>>>
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}
