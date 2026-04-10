using Domain.Entities;
using Domain.Interfaces.Repository;
using Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class VehicleRepository(ZeloFrotaDbContext context) : IVehicleRepository
{
    private readonly ZeloFrotaDbContext _context = context;

    public async Task<IEnumerable<Vehicle>> AllAsync(int skip, int take = 10)
    {
        return await _context.Vehicles
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
    }

    public async Task<Vehicle?> FindAsync(Guid id)
    {
        return await _context.Vehicles.FindAsync(id);
    }

    public async Task AddAsync(Vehicle value)
    {
        await _context.Vehicles.AddAsync(value);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(Vehicle value)
    {
        _context.Vehicles.Update(value);
        await _context.SaveChangesAsync();
    }

    public async Task<Vehicle?> GetByPlateAsync(string plate)
    {
        return await _context.Vehicles
                    .Where(v => v.Plate == plate)
                    .FirstOrDefaultAsync();
    }
}
