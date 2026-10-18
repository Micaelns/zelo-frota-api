using Application.Contracts.Abstractions.Vehicles.Query;
using Application.DTO.Vehicle;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Queries;

public class VehicleQuery(ZeloFrotaDbContext context) : IVehicleQuery
{
    private readonly ZeloFrotaDbContext _context = context;

    public async Task<(IEnumerable<VehicleDTO>, int totalItems)> AllAsync(int page, int take)
    {
        var skip = (page - 1) * take;
        var query = _context.Vehicles.AsNoTracking();

        int totalItems = await query.CountAsync(); 

        var items = await query
                    .Include(v => v.VehicleType)
                    .OrderByDescending(element => element.Plate)
                    .ThenBy(x => x.Id) // se tiver mais de uma Plate então por Id sempre
                    .Select(v => new VehicleDTO
                    {
                        Id = v.Id,
                        Plate = v.Plate,
                        VehicleType = v.VehicleType.Name,
                        RequeridLicenseCategory = v.VehicleType.RequeridLicenseCategory.ToString(),
                        Mileage = v.Mileage,
                        QtdTravels = v.Travels.Count()
                    })
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

        return (items, totalItems);
    }
}
