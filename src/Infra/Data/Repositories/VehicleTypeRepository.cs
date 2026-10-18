using Domain.Entities;
using Domain.Interfaces.Repository;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class VehicleTypeRepository(ZeloFrotaDbContext context) : IVehicleTypeRepository
{
    private readonly ZeloFrotaDbContext _context = context;

    public async Task<(IEnumerable<VehicleType>, int totalItems)> AllAsync(int page, int take = 10)
    {
        var skip = (page - 1) * take;
        var query = _context.VehicleTypes.AsNoTracking();

        int totalItems = await query.CountAsync();

        var items = await query
                    .OrderByDescending(element => element.Name)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

        return (items, totalItems);
    }

    public async Task<VehicleType?> FindAsync(Guid id)
    {
        return await _context.VehicleTypes.FindAsync(id);
    }

    public async Task UpdateAsync(VehicleType value)
    {
        _context.VehicleTypes.Update(value);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(VehicleType value)
    {
        await _context.VehicleTypes.AddAsync(value);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLogicalAsync(Guid id)
    {
        await _context.VehicleTypes
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(
                    x => x.Deleted,
                    DateTime.UtcNow));
    }
}
