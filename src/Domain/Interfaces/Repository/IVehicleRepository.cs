using Domain.Entities;
using Domain.ObjectValues;

namespace Domain.Interfaces.Repository;

public interface IVehicleRepository : IBaseRepository<Vehicle>
{
    public Task<Vehicle?> GetByPlateAsync(string plate);
}
