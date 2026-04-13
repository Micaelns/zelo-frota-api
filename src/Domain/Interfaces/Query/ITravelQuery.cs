namespace Domain.Interfaces.Query;

public interface ITravelQuery
{
    public Task<bool> HasOpenTravelInVehicle(Guid vehicleId);
}
