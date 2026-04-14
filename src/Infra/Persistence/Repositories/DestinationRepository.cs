using Domain.Entities;
using Domain.Interfaces.Repository;
using Infra.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Persistence.Repositories;

public class DestinationRepository(ZeloFrotaDbContext context) : IDestinationRepository
{
    private readonly ZeloFrotaDbContext _context = context;

    public async Task<IEnumerable<Destination>> AllAsync(int skip, int take = 10)
    {
        return await _context.Destinations
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
}
