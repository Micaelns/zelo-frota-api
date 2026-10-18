using Application.DTO.Travel;
using Domain.Entities;

namespace Application.Contracts.Abstractions.Travels.Query;

public interface ITravelQuery
{
    public Task<int> GetTravelsByVehicleContAsync(Guid vehicleId);
    public Task<IEnumerable<TravelDTO>> GetTravelsByVehicleAsync(Guid vehicleId, int page, int take = 10);
    public Task<IEnumerable<VehicleEconomyDto>> GetRankingVehicleEconomyAsync(bool orderByDescending, int take = 10);
    public Task<IEnumerable<RankingTravelDriverDTO>> GetRankingTravelDriverAsync(bool orderByDescending, int take = 10);
    public Task<IEnumerable<VehicleMileageRankingDTO>> GetRankingMileageAsync(bool orderByDescending, int take = 10);
    public Task<Travel?> FindAsync(Guid travelId);
    public Task<Travel?> GetOpenTravelInVehicleAsync(Guid vehicleId);
    public Task<bool> HasOpenTravelInVehicleAsync(Guid vehicleId);
}
