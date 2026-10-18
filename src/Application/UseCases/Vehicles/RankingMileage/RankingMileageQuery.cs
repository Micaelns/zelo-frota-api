using Application.DTO;
using Application.DTO.Travel;
using MediatR;

namespace Application.UseCases.Vehicles.RankingMileage;

public class RankingMileageQuery : IRequest<Result<List<VehicleMileageRankingDTO>>>
{
    public bool OrderByDescending { get; set; } = true;
    public int Take { get; set; } = 10;
}
