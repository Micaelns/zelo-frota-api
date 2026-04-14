using Domain.Entities;

namespace Domain.Interfaces.Query;

public interface ITravelQuery
{
    public Task<Travel?> GetOpenTravelInVehicle(Guid vehicleId);
    public Task<bool> HasOpenTravelInVehicle(Guid vehicleId);
}
