using Domain.Constants;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Seeds;

public class TravelCleanSeed
{
    public static async Task SeedCleanAsync(ZeloFrotaDbContext context)
    {
        var deleted = await context.Travels
            .Where(t => t.Vehicle.VehicleType.Name == SeedConstants.DefaultVehicleType)
            .ExecuteDeleteAsync();

        if (deleted == 0) return;

        await context.SaveChangesAsync();

        Console.WriteLine($"- Limpo {deleted} registros [TravelCleanSeed]");
    }
}
