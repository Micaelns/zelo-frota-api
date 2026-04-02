using Domain.Entities;
using Domain.Interfaces.Repository;
using Domain.ObjectValues;

namespace Infra.Repositories.Fakes;

public class VehicleFakeRepository : IVehicleRepository
{
    private readonly List<Vehicle> _vehicles = [];

    public async Task<IEnumerable<Vehicle>> All(int skip, int take = 10)
    {
        await Task.CompletedTask;
        return _vehicles.Skip(skip).Take(take);
    }

    public async Task Delete(Guid id)
    {
        var element = _vehicles.FirstOrDefault(item => item.Id == id);
        if (element != null)
        {
            _vehicles.Remove(element);
        }
        await Task.CompletedTask;
    }

    public Task<Vehicle?> Find(Guid id)
    {
        var vehicle = _vehicles.FirstOrDefault(v => v.Id == id);
        return Task.FromResult(vehicle);
    }

    public Task<Vehicle?> GetByPlate(Plate plate)
    {
        var vehicle = _vehicles.FirstOrDefault(v => v.Plate == plate.Value);
        return Task.FromResult(vehicle);
    }

    public async Task Save(Vehicle value)
    {
        _vehicles.Add(value);
        await Task.CompletedTask;
    }
}
