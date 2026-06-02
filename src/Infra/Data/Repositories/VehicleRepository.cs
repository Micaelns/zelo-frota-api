using Application.DTO.Vehicle;
using Azure;
using Domain.Entities;
using Domain.Interfaces.Repository;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public class VehicleRepository(ZeloFrotaDbContext context) : IVehicleRepository
{
    private readonly ZeloFrotaDbContext _context = context;


    public async Task<int> AllContAsync()
    {
        return await _context.Vehicles
                    .CountAsync();
    }
    public async Task<IEnumerable<Vehicle>> AllAsync(int page, int take = 10)
    {
        var skip = (page - 1) * take;
        return await _context.Vehicles
                    .AsNoTracking()
                    .Include("VehicleType")
                    .OrderByDescending(element => element.Plate)
                    .ThenBy(x => x.Id) // se tiver mais de uma Plate então por Id sempre 
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

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Vehicle?> GetByPlateAsync(string plate)
    {
        return await _context.Vehicles
                    .Where(v => v.Plate == plate)
                    .FirstOrDefaultAsync();
    }

    public Task DeleteLogicalAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
