using Application.Contracts.Abstractions.Travels.Query;
using Application.DTO.Travel;
using Domain.Entities;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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

    private IQueryable<Travel> QueryTravelComplete()
    {
        return _context.Travels
            .Include(c => c.Destination)
            .Include(t => t.Vehicle)
            .ThenInclude(v => v.VehicleType);
    }

    public async Task<Travel?> GetOpenTravelInVehicleAsync(Guid vehicleId)
    {
        return await QueryTravelComplete()
            .FirstOrDefaultAsync(element => element.VehicleId == vehicleId && element.End == null);
    }

    public async Task<Travel?> FindAsync(Guid travelId)
    {
        return await QueryTravelComplete()
           .FirstOrDefaultAsync(element => element.Id == travelId);
    }

    public async Task<IEnumerable<Travel>> GetTravelsByVehicleAsync(Guid vehicleId, int skip, int take = 10)
    {
        skip = Math.Max(0, skip);
        take = Math.Clamp(take, 1, 1000);

        return await _context.Travels
            .AsNoTracking()
            .Where(element => element.VehicleId == vehicleId)
            .OrderByDescending(element => element.Start)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleEconomyDto>> GetHankingVehicleEconomyAsync(int skip, int take = 10)
    {
        return  await _context.Travels
                .AsNoTracking()
                .Where(t => t.Autonomy.HasValue)
                .GroupBy(t => new
                {
                    t.VehicleId,
                    t.Vehicle.Plate,
                    VehicleType = t.Vehicle.VehicleType.Name
                })
                .Select(group => new VehicleEconomyDto
                {
                    VehicleId = group.Key.VehicleId,

                    VehiclePlate = group.Key.Plate,

                    VehicleType = group.Key.VehicleType,

                    AverageAutonomy = group.Average(t =>
                        (double)t.Autonomy!.Value),

                    TotalTravels = group.Count()
                })
                .OrderByDescending(x => x.AverageAutonomy)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
    }
}
