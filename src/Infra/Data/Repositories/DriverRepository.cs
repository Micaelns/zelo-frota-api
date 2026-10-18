using Domain.Entities;
using Domain.Interfaces.Repository;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class DriverRepository(ZeloFrotaDbContext context) : IDriverRepository
{
    private ZeloFrotaDbContext _context = context;

    public async Task AddAsync(Driver value)
    {
        await _context.Drivers.AddAsync(value);
        await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<Driver>, int totalItems)> AllAsync(int page, int take)
    {
        var skip = (page - 1) * take;
        var query = _context.Drivers.AsNoTracking();

        int totalItems = await query.CountAsync();

        var items = await query
                    .Include("DriverLicense")
                    .OrderByDescending(element => element.Name)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

        return (items, totalItems);
    }

    public async Task DeleteLogicalAsync(Guid id)
    {
        await _context.Drivers
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(
                    x => x.Deleted,
                    DateTime.UtcNow));
    }

    public async Task<Driver?> FindAsync(Guid id)
    {
        return await _context.Drivers.FindAsync(id);
    }

    public async Task UpdateAsync(Driver value)
    {
        _context.Drivers.Update(value);
        await _context.SaveChangesAsync();
    }
}
