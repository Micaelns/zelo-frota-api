using Domain.Entities;
using Domain.Interfaces.Repository;

namespace Infra.Repositories.Fakes;

public class TravelFakeRepository : ITravelRepository
{
    private readonly List<Travel> _travels = [];
    public async Task<IEnumerable<Travel>> All(int skip, int take = 10)
    {
        await Task.CompletedTask;
        return _travels.Skip(skip).Take(take);
    }

    public async Task Delete(Guid id)
    {
        var element = _travels.FirstOrDefault(item => item.Id == id);
        if (element != null)
        {
            _travels.Remove(element);
        }
        await Task.CompletedTask;
    }

    public Task<Travel?> Find(Guid id)
    {
        var vehicle = _travels.FirstOrDefault(v => v.Id == id);
        return Task.FromResult(vehicle);
    }

    public async Task Save(Travel value)
    {
        _travels.Add(value);
        await Task.CompletedTask;
    }
}
