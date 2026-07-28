using Application.DTO.Travel;
using Domain.Entities;

namespace Application.Contracts.Abstractions.Travels.Query;

public interface ITravelQuery
{
    public Task<int> GetTravelsByVehicleContAsync(Guid vehicleId);
    public Task<IEnumerable<Travel>> GetTravelsByVehicleAsync(Guid vehicleId, int page, int take = 10);
    public Task<IEnumerable<VehicleEconomyDto>> GetHankingVehicleEconomyAsync(int page, int take = 10);
    public Task<IEnumerable<VehicleMileageRankingDTO>> GetMileageHankingAsync(bool orderByDescending, int skip, int take = 10);
    public Task<Travel?> FindAsync(Guid travelId);
    public Task<Travel?> GetOpenTravelInVehicleAsync(Guid vehicleId);
    public Task<bool> HasOpenTravelInVehicleAsync(Guid vehicleId);
}
