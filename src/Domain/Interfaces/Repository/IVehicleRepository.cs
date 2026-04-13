using Domain.Entities;

namespace Domain.Interfaces.Repository;

public interface IVehicleRepository : IBaseRepository<Vehicle>
{
    public Task<Vehicle?> GetByPlateAsync(string plate);
    public Task SaveChangesAsync();
}
