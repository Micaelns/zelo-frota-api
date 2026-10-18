using Application.Contracts.Abstractions.Travels.Query;
using Application.DTO.Travel;
using Application.Helpers;
using Domain.Entities;
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

    private IQueryable<Travel> QueryTravelComplete()
    {
        return _context.Travels
            .Include(c => c.Destination)
            .Include(c => c.Driver)
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


    public async Task<int> GetTravelsByVehicleContAsync(Guid vehicleId)
    {
        return await _context.Travels
                    .Where(element => element.VehicleId == vehicleId)
                    .CountAsync();
    }
    public async Task<IEnumerable<TravelDTO>> GetTravelsByVehicleAsync(Guid vehicleId, int page, int take = 10)
    {
        var skip = Math.Max(0, (page - 1) * take);
        take = Math.Clamp(take, 1, 1000);

        return await QueryTravelComplete()
            .AsNoTracking()
            .Where(element => element.VehicleId == vehicleId)
            .OrderByDescending(element => element.Start)
            .Select(travel => new TravelDTO
            {
                IdTravel = travel.Id,
                VehiclePlate = travel.Vehicle.Plate,
                VehicleId = travel.Vehicle.Id,
                Vehicle = travel.Vehicle.VehicleType.Name,
                Driver = travel.Driver != null ? travel.Driver.Name : null,
                DestinationId = travel.Destination.Id,
                Destination = travel.Destination.ToString(),
                Autonomy = travel.Autonomy,
                StartedMileage = travel.StartedMileage,
                Start = TimeZoneHelper.ToSaoPaulo(travel.Start),
                FinishedMileage = travel.FinishedMileage,
                End = TimeZoneHelper.ToSaoPaulo(travel.End)
            })
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleEconomyDto>> GetRankingVehicleEconomyAsync(bool orderByDescending, int take = 10)
    {
        var sqlQuery = _context.Travels
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
                });

        if (orderByDescending)
        {
            sqlQuery = sqlQuery.OrderByDescending(x => x.AverageAutonomy);
        }
        else
        {
            sqlQuery = sqlQuery.OrderBy(x => x.AverageAutonomy);
        }

        return await sqlQuery
                   .Take(take)
                   .ToListAsync();
    }

    public async Task<IEnumerable<VehicleMileageRankingDTO>> GetRankingMileageAsync(bool orderByDescending, int take = 10)
    {
        var sqlQuery = _context.Travels
                .AsNoTracking()
                .Where(t => t.FinishedMileage.HasValue && t.StartedMileage!.HasValue)
                .GroupBy(t => new
                {
                    t.VehicleId,
                    t.Vehicle.Plate,
                    VehicleType = t.Vehicle.VehicleType.Name
                })
                .Select(group => new VehicleMileageRankingDTO
                {
                    VehicleId = group.Key.VehicleId,

                    VehiclePlate = group.Key.Plate,

                    VehicleType = group.Key.VehicleType,

                    TotalMileage = group.Sum(t =>
                        (int)t.FinishedMileage!.Value - (int)t.StartedMileage!.Value),

                    TotalTravels = group.Count()
                });

        if (orderByDescending)
        {
            sqlQuery = sqlQuery.OrderByDescending(x => x.TotalMileage);
        }
        else
        {
            sqlQuery = sqlQuery.OrderBy(x => x.TotalMileage);
        }


        return await sqlQuery
                    .Take(take)
                    .ToListAsync();
    }

    public async Task<IEnumerable<RankingTravelDriverDTO>> GetRankingTravelDriverAsync(bool orderByDescending, int take = 10)
    {
        var sqlQuery = _context.Travels
               .AsNoTracking()
               .Where(t => t.DriverId.HasValue && t.Driver != null)
               .GroupBy(group => new
               {
                   group.DriverId,
                   group.VehicleId,
                   group.Vehicle.Plate,
                   VehicleType = group.Vehicle.VehicleType.Name,
                   DriverName = group.Driver!.Name
               })
               .Select(group => new RankingTravelDriverDTO
               {
                   DriverName = group.Key.DriverName,
                   VehicleId = group.Key.VehicleId,
                   VehiclePlate = group.Key.Plate,
                   VehicleType = group.Key.VehicleType,
                   TotalMileage = group.Sum(t =>
                        (int)t.FinishedMileage!.Value - (int)t.StartedMileage!.Value),
                   TotalTravels = group.Count(),
                   DateFirstTravel = group.Min(m => m.Start),
                   DateLastTravel = group.Max(m => m.End)
               });

        if (orderByDescending)
        {
            sqlQuery = sqlQuery.OrderByDescending(x => x.TotalTravels);
        }
        else
        {
            sqlQuery = sqlQuery.OrderBy(x => x.TotalTravels);
        }

        var tmp = sqlQuery.Take(take).ToQueryString();

        return await sqlQuery
                    .Take(take)
                    .ToListAsync();
    }
}
