using Domain.Entities;
using Domain.Interfaces.Query;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Queries;

public class TravelQuery(ZeloFrotaDbContext context) : ITravelQuery
{
    private readonly ZeloFrotaDbContext _context = context;

    public async Task<bool> HasOpenTravelInVehicleAsync(Guid vehicleId)
    {
        return await _context.Travels
            .AsNoTracking()
            .AnyAsync(element => element.VehicleId == vehicleId && element.End == null);
    }

    public async Task<Travel?> GetOpenTravelInVehicleAsync(Guid vehicleId)
    {
        return await _context.Travels
            .FirstOrDefaultAsync(element => element.VehicleId == vehicleId && element.End == null);
    }

    public async Task<IEnumerable<Travel>> GetTravelsByVehicleAsync(Guid vehicleId, int skip, int take = 10)
    {
        skip = Math.Max(0, skip);
        take = Math.Clamp(take, 1, 1000);

        return await _context.Travels
            .AsNoTracking()
            .Where(element => element.VehicleId == vehicleId)
            .OrderByDescending( element => element.Start)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}
