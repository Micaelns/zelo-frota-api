using Domain.Entities;
using Domain.Interfaces.Repository;

namespace Infra.Repositories.Fakes;

public class DestinationFakeRepository : IDestinationRepository
{
    private readonly List<Destination> _destinations = [];

    public async Task<IEnumerable<Destination>> All(int skip, int take = 10)
    {
        await Task.CompletedTask;
        return _destinations.Skip(skip).Take(take);
    }

    public async Task Delete(Guid id)
    {
        var element = _destinations.FirstOrDefault(item => item.Id == id);
        if (element != null)
        {
            _destinations.Remove(element);
        }
        await Task.CompletedTask;
    }

    public Task<Destination?> Find(Guid id)
    {
        var vehicle = _destinations.FirstOrDefault(v => v.Id == id);
        return Task.FromResult(vehicle);
    }

    public async Task Save(Destination value)
    {
        _destinations.Add(value);
        await Task.CompletedTask;
    }
}
