using Application.DTO;
using Application.DTO.Travel;
using MediatR;

namespace Application.UseCases.Vehicles.RankingEconomyVehicle;

public class RankingEconomyQuery : IRequest<Result<List<VehicleEconomyDto>>>
{
    public bool OrderByDescending { get; set; } = true;
    public int Take { get; set; } = 10;
}
