using Domain.Entities;
using Domain.Interfaces.Repository;
using Infra.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Persistence.Repositories;

public class VehicleTypeRepository(ZeloFrotaDbContext context) : IVehicleTypeRepository
{
    private readonly ZeloFrotaDbContext _context = context;

    public async Task<IEnumerable<VehicleType>> AllAsync(int skip, int take = 10)
    {
        return await _context.VehicleTypes
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync(); 
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
}
