using Domain.Entities;

namespace Domain.Interfaces.Repository;

public interface IVehicleTypeRepository : IBaseRepository<VehicleType>
{
    public Task<int> AllContAsync();
}
