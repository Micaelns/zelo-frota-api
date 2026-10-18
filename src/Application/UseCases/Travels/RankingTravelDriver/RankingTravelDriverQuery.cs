using Application.DTO;
using Application.DTO.Travel;
using MediatR;

namespace Application.UseCases.Travels.RankingTravelDriver;

public class RankingTravelDriverQuery : IRequest<Result<List<RankingTravelDriverDTO>>>
{
    public bool OrderByDescending { get; set; } = true;
    public int Take { get; set; } = 10;
}
