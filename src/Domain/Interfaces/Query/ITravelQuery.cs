using Domain.Entities;

namespace Domain.Interfaces.Query;

public interface ITravelQuery
{
    public Task<IEnumerable<Travel>> GetTravelsByVehicleAsync(Guid vehicleId, int skip, int take = 10);
    public Task<Travel?> GetOpenTravelInVehicleAsync(Guid vehicleId);
    public Task<bool> HasOpenTravelInVehicleAsync(Guid vehicleId);
}
