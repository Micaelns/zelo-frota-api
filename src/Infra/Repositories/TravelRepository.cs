using Domain.Entities;
using Domain.Interfaces.Repository;
using Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class TravelRepository(ZeloFrotaDbContext context) : ITravelRepository
{
    private readonly ZeloFrotaDbContext _context = context;

    public async Task<IEnumerable<Travel>> AllAsync(int skip, int take = 10)
    {
        return await _context.Travels
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
    }

    public async Task<Travel?> FindAsync(Guid id)
    {
        return await _context.Travels.FindAsync(id);
    }

    public async Task AddAsync(Travel value)
    {
        await _context.Travels.AddAsync(value);
        await _context.SaveChangesAsync();
    }
}
