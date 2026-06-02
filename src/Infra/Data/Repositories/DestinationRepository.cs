using Azure;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class DestinationRepository(ZeloFrotaDbContext context) : IDestinationRepository
{
    private readonly ZeloFrotaDbContext _context = context;

    public async Task<int> AllContAsync()
    {
        return await _context.Destinations
                    .CountAsync();
    }

    public async Task<IEnumerable<Destination>> AllAsync(int page, int take = 10)
    {
        var skip = (page - 1) * take;
        return await _context.Destinations
                    .OrderByDescending(element => element.ZipCode)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
    }

    public async Task<Destination?> FindAsync(Guid id)
    {
        return await _context.Destinations.FindAsync(id);
    }

    public async Task AddAsync(Destination value)
    {
        await _context.Destinations.AddAsync(value);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Destination value)
    {
        _context.Destinations.Update(value);
        await _context.SaveChangesAsync();
    }

    public async Task<Destination?> GetByZipCodeAsync(string zipCode)
    {
        return await _context.Destinations
                    .Where(v => v.ZipCode == zipCode)
                    .FirstOrDefaultAsync();
    }

    public Task DeleteLogicalAsync(Guid id)
    {
        throw new NotImplementedException();
    }

}
