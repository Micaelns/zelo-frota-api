using Domain.Entities;

namespace Domain.Interfaces.Repository;

public interface ITravelRepository : IBaseRepository<Travel>
{
    public Task<IEnumerable<Travel>> GetTravelsByVehicleAsync(Guid vehicleId, int skip, int take = 10);
}
