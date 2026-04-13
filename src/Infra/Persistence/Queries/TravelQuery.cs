using Domain.Interfaces.Query;
using Infra.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Persistence.Queries;

public class TravelQuery(ZeloFrotaDbContext context) : ITravelQuery
{
    private readonly ZeloFrotaDbContext _context = context;

    public async Task<bool> HasOpenTravelInVehicle(Guid vehicleId)
    {
        return await _context.Travels
            .AsNoTracking()
            .AnyAsync(element => element.VehicleId == vehicleId && element.End == null);
    }
}
