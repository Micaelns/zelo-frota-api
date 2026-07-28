using Application.DTO;
using Application.DTO.Travel;
using MediatR;

namespace Application.UseCases.Vehicles.MileageRanking;

public class MileageRankingQuery : IRequest<Result<List<VehicleMileageRankingDTO>>>
{
    public bool OrderByDescending { get; set; } = true;
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
}
