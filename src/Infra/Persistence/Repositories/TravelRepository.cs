using Domain.Entities;
using Domain.Interfaces.Repository;
using Infra.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Persistence.Repositories;

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

    public async Task UpdateAsync(Travel value)
    {
        _context.Travels.Update(value);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Travel>> GetTravelsByVehicleAsync(Guid vehicleId, int skip, int take = 10)
    {
        return await _context.Travels
                    .Where(v => v.VehicleId == vehicleId)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
    }
}
